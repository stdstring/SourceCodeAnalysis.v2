using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Processors
{
    internal class SolutionProcessor : ISourceProcessor
    {
        public SolutionProcessor(String solutionFilename, OutputImpl output)
        {
            if (String.IsNullOrEmpty(solutionFilename))
                throw new ArgumentNullException(nameof(solutionFilename));
            _solutionFilename = Path.GetFullPath(solutionFilename);
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the solution {_solutionFilename} is started");
            if (DotnetHelper.Build(_solutionFilename).ExitCode != 0)
                throw new InvalidOperationException();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            if (!File.Exists(_solutionFilename))
            {
                _output.WriteFailLine($"Bad (unknown) target {_solutionFilename}");
                return false;
            }
            Solution solution = workspace.OpenSolutionAsync(_solutionFilename).Result;
            Boolean result = true;
            foreach (Project project in solution.Projects.OrderBy(project => project.Name))
            {
                _output.WriteInfoLine($"Processing of the project {project.FilePath} is started");
                ProjectProcessor projectProcessor = new ProjectProcessor(project.FilePath, _output);
                result &= projectProcessor.Process(project, analyzers);
                _output.WriteInfoLine($"Processing of the project {project.FilePath} is finished");
            }
            _output.WriteInfoLine($"Processing of the solution {_solutionFilename} is finished");
            return result;
        }

        private readonly String _solutionFilename;
        private readonly OutputImpl _output;
    }
}