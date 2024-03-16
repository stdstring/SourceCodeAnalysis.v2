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
            _solutionFilename = solutionFilename;
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the solution {_solutionFilename} is started");
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            if (!File.Exists(_solutionFilename))
            {
                _output.WriteFailLine($"Bad (unknown) target {_solutionFilename}");
                return false;
            }
            Solution solution = workspace.OpenSolutionAsync(_solutionFilename).Result;
            Boolean result = true;
            foreach (ProjectId projectId in solution.GetProjectDependencyGraph().GetTopologicallySortedProjects())
            {
                Project? project = solution.GetProject(projectId);
                if (project == null)
                    throw new InvalidOperationException();
                result &= Process(project, analyzers);
            }
            _output.WriteInfoLine($"Processing of the solution {_solutionFilename} is finished");
            return result;
        }

        private Boolean Process(Project project, IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the project {project.FilePath} is started");
            ProjectProcessor projectProcessor = new ProjectProcessor(project.FilePath, _output);
            Boolean result = projectProcessor.Process(project, analyzers);
            _output.WriteInfoLine($"Processing of the project {project.FilePath} is finished");
            return result;
        }

        private readonly String _solutionFilename;
        private readonly OutputImpl _output;
    }
}