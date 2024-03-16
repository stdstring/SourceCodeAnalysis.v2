using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;
using System.Diagnostics;

namespace SourceCodeCheckApp.Processors
{
    internal class ProjectProcessor : ISourceProcessor
    {
        public ProjectProcessor(String? projectFilename, OutputImpl output)
        {
            if (String.IsNullOrEmpty(projectFilename))
                throw new ArgumentNullException(nameof(projectFilename));
            if (!File.Exists(projectFilename))
                throw new ArgumentException($"Bad (unknown) target {projectFilename}");
            _projectFilename = projectFilename;
            _output = output;
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the project {_projectFilename} is started");
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
            if (!RestoreNuget(project.FilePath))
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
                SyntaxTree? syntaxTree = sourceFile.GetSyntaxTreeAsync().Result;
                if (syntaxTree == null)
                    throw new InvalidOperationException();
                result &= ProcessFile(sourceFile.FilePath, syntaxTree, compilation.GetSemanticModel(syntaxTree), analyzers);
            }
            return result;
        }

        private Boolean RestoreNuget(String projectPath)
        {
            // It seems that the best solution to restore nuget packages is to use "dotnet restore" command.
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"restore {projectPath}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += delegate (object _, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        _output.WriteInfoLine($"{e.Data}");
                };
                process.ErrorDataReceived += delegate (object _, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        _output.WriteErrorLine($"{e.Data}");
                };
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
                catch (Exception ex)
                {
                    _output.WriteErrorLine($"{ex.Message}");
                    return false;
                }
            }
        }

        private Boolean ProcessFile(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            FileProcessor fileProcessor = new FileProcessor(filename, _output);
            return fileProcessor.Process(tree, model, analyzers);
        }

        private readonly String _projectFilename;
        private readonly OutputImpl _output;
    }
}