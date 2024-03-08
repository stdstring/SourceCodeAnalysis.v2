using Microsoft.Build.Locator;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Processors
{
    internal static class SourceProcessorFactory
    {
        public static ISourceProcessor Create(String source, IConfig externalConfig, OutputImpl output)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            String sourceExtension = Path.GetExtension(source);
            if (String.IsNullOrEmpty(sourceExtension) || !ProcessorsMap.ContainsKey(sourceExtension))
                throw new ArgumentException(nameof(source));
            MSBuildLocator.RegisterDefaults();
            return ProcessorsMap[sourceExtension](source, externalConfig, output);
        }

        private static readonly IDictionary<String, Func<String, IConfig, OutputImpl, ISourceProcessor>> ProcessorsMap = new Dictionary<String, Func<String, IConfig, OutputImpl, ISourceProcessor>>
        {
            {".sln", (source, config, output) => new SolutionProcessor(source, config, output)},
            {".csproj", (source, config, output) => new ProjectProcessor(source, config, output)},
            {".cs", (source, config, output) => new FileProcessor(source, config, output)}
        };
    }
}