using System.Diagnostics;
using System.Reflection;
using System.Text;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckAppTests.Utils
{
    internal static class ExecutionHelper
    {
        public static ExecutionResult Execute(String target, String config, OutputLevel outputLevel)
        {
            return Execute(target, config, outputLevel, new Dictionary<String, String>());
        }

        public static ExecutionResult Execute(String target, String config, OutputLevel outputLevel, IDictionary<String, String> environmentVariables)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException(nameof(target));
            String arguments = CreateArgList(target, config, outputLevel);
            return Execute(arguments, environmentVariables);
        }

        public static ExecutionResult Execute(String arguments)
        {
            return Execute(arguments, new Dictionary<String, String>());
        }

        public static ExecutionResult Execute(String arguments, IDictionary<String, String> environmentVariables)
        {
            using (Process utilProcess = new Process())
            {
                String currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                utilProcess.StartInfo.FileName = Path.Combine(currentDir, UtilFilename);
                utilProcess.StartInfo.Arguments = arguments;
                utilProcess.StartInfo.UseShellExecute = false;
                utilProcess.StartInfo.CreateNoWindow = true;
                utilProcess.StartInfo.RedirectStandardError = true;
                utilProcess.StartInfo.RedirectStandardOutput = true;
                utilProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                utilProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                utilProcess.StartInfo.WorkingDirectory = currentDir;
                foreach (KeyValuePair<String, String> environmentVariable in environmentVariables)
                    utilProcess.StartInfo.Environment.Add(environmentVariable.Key, environmentVariable.Value);
                IList<String> output = new List<String>();
                IList<String> error = new List<String>();
                utilProcess.OutputDataReceived += (sender, e) => { output.Add(e.Data); };
                utilProcess.ErrorDataReceived += (sender, e) => { error.Add(e.Data); };
                utilProcess.Start();
                utilProcess.BeginErrorReadLine();
                utilProcess.BeginOutputReadLine();
                utilProcess.WaitForExit();
                return new ExecutionResult(utilProcess.ExitCode, String.Join("\r\n", output), String.Join("\r\n", error));
            }
        }

        private static String CreateArgList(String target, String config, OutputLevel outputLevel)
        {
            StringBuilder dest = new StringBuilder();
            dest.AppendFormat("--source=\"{0}\"", target);
            if (!String.IsNullOrEmpty(config))
                dest.AppendFormat(" --config=\"{0}\"", config);
            dest.AppendFormat(" --output-level={0}", outputLevel);
            return dest.ToString();
        }

        private const String UtilFilename = "SourceCodeCheckApp.exe";
    }
}