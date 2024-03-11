using Microsoft.Build.Locator;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Processors
{
    internal static class SourceProcessorFactory
    {
        public static ISourceProcessor Create(String source, OutputImpl output)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            String sourceExtension = Path.GetExtension(source);
            if (String.IsNullOrEmpty(sourceExtension) || !ProcessorsMap.ContainsKey(sourceExtension))
                throw new ArgumentException(nameof(source));
            MSBuildLocator.RegisterDefaults();
            return ProcessorsMap[sourceExtension](source, output);
        }

        private static readonly IDictionary<String, Func<String, OutputImpl, ISourceProcessor>> ProcessorsMap = new Dictionary<String, Func<String, OutputImpl, ISourceProcessor>>
        {
            {".sln", (source, output) => new SolutionProcessor(source, output)},
            {".csproj", (source, output) => new ProjectProcessor(source, output)},
            {".cs", (source, output) => new FileProcessor(source, output)}
        };
    }
}