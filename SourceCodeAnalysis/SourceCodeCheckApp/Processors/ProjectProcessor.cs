using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Processors
{
    internal class ProjectProcessor : ISourceProcessor
    {
        public ProjectProcessor(String projectFilename, IConfig externalConfig, OutputImpl output)
        {
            if (String.IsNullOrEmpty(projectFilename))
                throw new ArgumentNullException(nameof(projectFilename));
            _projectFilename = projectFilename;
            _output = output;
            _processorHelper = new ProjectProcessorHelper(externalConfig, output);
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the project {_projectFilename} is started");
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            if (!File.Exists(_projectFilename))
            {
                _output.WriteFailLine($"Bad (unknown) target {_projectFilename}");
                return false;
            }
            Project project = workspace.OpenProjectAsync(_projectFilename).Result;
            Boolean result = _processorHelper.ProcessProject(project, analyzers, Process);
            _output.WriteInfoLine($"Processing of the project {_projectFilename} is finished");
            return result;
        }

        private Boolean Process(Document file, Compilation compilation, ConfigData externalData, IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the file {file.FilePath} is started");
            SyntaxTree tree = file.GetSyntaxTreeAsync().Result;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = _processorHelper.ProcessFile(file.FilePath, tree, model, externalData, analyzers);
            _output.WriteInfoLine($"Processing of the file {file.FilePath} is finished");
            return result;
        }

        private readonly String _projectFilename;
        private readonly OutputImpl _output;
        private readonly ProjectProcessorHelper _processorHelper;
    }
}