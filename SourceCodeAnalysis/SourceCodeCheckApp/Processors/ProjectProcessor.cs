using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Processors
{
    internal class ProjectProcessor : ISourceProcessor
    {
        public ProjectProcessor(String? projectFilename, IOutput output)
        {
            if (String.IsNullOrEmpty(projectFilename))
                throw new ArgumentNullException(nameof(projectFilename));
            if (!File.Exists(projectFilename))
                throw new ArgumentException($"Bad (unknown) target {projectFilename}");
            _projectFilename = Path.GetFullPath(projectFilename);
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the project {_projectFilename} is started");
            DotnetUtilityService.Build(_projectFilename, _output);
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(_projectFilename).Result;
            Boolean result = Process(project, analyzers);
            _output.WriteInfoLine($"Processing of the project {_projectFilename} is finished");
            return result;
        }

        public Boolean Process(Project project, IList<IFileAnalyzer> analyzers)
        {
            if (project.FilePath == null)
                throw new InvalidOperationException();
            Compilation? compilation = project.GetCompilationAsync().Result;
            if (compilation == null)
                throw new InvalidOperationException();
            if (!CompilationChecker.CheckCompilationErrors(project.FilePath, compilation, _output))
                return false;
            Boolean result = true;
            foreach (Document sourceFile in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular))
            {
                if (sourceFile.FilePath == null)
                    throw new InvalidOperationException();
                if (ProjectIgnoredFiles.IgnoreFile(sourceFile.FilePath))
                    continue;
                String filePath = sourceFile.FilePath;
                _output.WriteInfoLine($"Processing of the file {filePath} is started");
                SyntaxTree? syntaxTree = sourceFile.GetSyntaxTreeAsync().Result;
                if (syntaxTree == null)
                    throw new InvalidOperationException();
                result &= ProcessFile(filePath, syntaxTree, compilation.GetSemanticModel(syntaxTree), analyzers);
                _output.WriteInfoLine($"Processing of the file {filePath} is finished");
            }
            return result;
        }

        private Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            FileProcessor fileProcessor = new FileProcessor(filename);
            return fileProcessor.Process(tree, model, analyzers);
        }

        private readonly String _projectFilename;
        private readonly IOutput _output;
    }
}