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
            if (DotnetHelper.Build(_projectFilename).ExitCode != 0)
                throw new InvalidOperationException();
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
            FileProcessor fileProcessor = new FileProcessor(filename, _output);
            return fileProcessor.Process(tree, model, analyzers);
        }

        private readonly String _projectFilename;
        private readonly IOutput _output;
    }

    internal record AppExecuteResult(Int32 ExitCode, String[] Output, String[] Error);

    internal static class DotnetHelper
    {
        public static AppExecuteResult Build(String path)
        {
            IList<String> output = new List<String>();
            IList<String> error = new List<String>();
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"build {path}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += delegate (object _, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        output.Add($"{e.Data}");
                };
                process.ErrorDataReceived += delegate (object _, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        error.Add($"{e.Data}");
                };
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    return new AppExecuteResult(ExitCode: process.ExitCode, Output: output.ToArray(), Error: error.ToArray());
                }
                catch (Exception ex)
                {
                    error.Add($"{ex.Message}");
                    return new AppExecuteResult(ExitCode: -1, Output: output.ToArray(), Error: error.ToArray());
                }
            }
        }
    }
}