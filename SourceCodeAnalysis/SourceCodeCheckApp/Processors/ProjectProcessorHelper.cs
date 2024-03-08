using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Managers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Processors
{
    internal class ProjectProcessorHelper
    {
        public ProjectProcessorHelper(IConfig externalConfig, OutputImpl output)
        {
            _externalConfig = externalConfig;
            _output = output;
            _fileProcessor = new FileProcessorHelper();
        }

        public Boolean ProcessProject(Project project, IList<IFileAnalyzer> analyzers, Func<Document, Compilation, ConfigData, IList<IFileAnalyzer>, Boolean> fileProcessor)
        {
            Compilation compilation = project.GetCompilationAsync().Result;
            if (!CompilationChecker.CheckCompilationErrors(project.FilePath, compilation, _output))
                return false;
            Boolean result = true;
            String projectDir = Path.GetDirectoryName(project.FilePath);
            ConfigData configData = _externalConfig.Load(project.Name);
            FileProcessingManager manager = new FileProcessingManager(configData);
            foreach (Document file in project.Documents.Where(doc => doc.SourceCodeKind == SourceCodeKind.Regular && !ProjectIgnoredFiles.IgnoreFile(doc.FilePath)))
            {
                String documentRelativeFilename = GetRelativeFilename(projectDir, file.FilePath);
                if (manager.SkipFileProcessing(documentRelativeFilename))
                    continue;
                result &= fileProcessor(file, compilation, configData, analyzers);
            }
            return result;
        }

        public Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, ConfigData externalData, IList<IFileAnalyzer> analyzers)
        {
            return _fileProcessor.Process(filename, tree, model, analyzers, externalData);
        }

        private String GetRelativeFilename(String projectDir, String filename)
        {
            return filename.Substring(projectDir.Length + 1);
        }

        private readonly IConfig _externalConfig;
        private readonly OutputImpl _output;
        private readonly FileProcessorHelper _fileProcessor;
    }
}