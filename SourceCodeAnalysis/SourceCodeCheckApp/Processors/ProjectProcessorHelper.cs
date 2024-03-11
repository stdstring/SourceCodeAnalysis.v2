using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Processors
{
    internal class ProjectProcessorHelper
    {
        public ProjectProcessorHelper(OutputImpl output)
        {
            _output = output;
            _fileProcessor = new FileProcessorHelper();
        }

        public Boolean ProcessProject(Project project, IList<IFileAnalyzer> analyzers, Func<Document, Compilation, IList<IFileAnalyzer>, Boolean> fileProcessor)
        {
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(project.FilePath, compilation, _output))
                return false;
            Boolean result = true;
            String projectDir = Path.GetDirectoryName(project.FilePath);
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                String documentRelativeFilename = GetRelativeFilename(projectDir, file.FilePath);
                result &= fileProcessor(file, compilation, analyzers);
            }
            return result;
        }

        public Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            return _fileProcessor.Process(filename, tree, model, analyzers);
        }

        private String GetRelativeFilename(String projectDir, String filename)
        {
            return filename.Substring(projectDir.Length + 1);
        }

        private readonly OutputImpl _output;
        private readonly FileProcessorHelper _fileProcessor;
    }
}