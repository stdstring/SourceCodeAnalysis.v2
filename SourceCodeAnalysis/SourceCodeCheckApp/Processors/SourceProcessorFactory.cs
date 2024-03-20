using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Processors
{
    internal static class SourceProcessorFactory
    {
        public static ISourceProcessor Create(String source, OutputImpl output)
        {
            String sourceExtension = Path.GetExtension(source);
            if (String.IsNullOrEmpty(sourceExtension) || !ProcessorsMap.ContainsKey(sourceExtension))
                throw new ArgumentException(nameof(source));
            return ProcessorsMap[sourceExtension](source, output);
        }

        public static Boolean IsSupportedSource(String source)
        {
            String sourceExtension = Path.GetExtension(source);
            return ProcessorsMap.ContainsKey(sourceExtension);
        }

        private static readonly IDictionary<String, Func<String, OutputImpl, ISourceProcessor>> ProcessorsMap = new Dictionary<String, Func<String, OutputImpl, ISourceProcessor>>
        {
            {".sln", (source, output) => new SolutionProcessor(source, output)},
            {".csproj", (source, output) => new ProjectProcessor(source, output)},
            {".cs", (source, output) => new FileProcessor(source, output)}
        };
    }
}