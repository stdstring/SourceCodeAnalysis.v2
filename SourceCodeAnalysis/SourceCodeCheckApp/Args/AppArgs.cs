using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Args
{
    internal class AppArgs
    {
        public AppArgs(AppUsageMode mode)
        {
            Mode = mode;
            Source = null;
        }

        public AppUsageMode Mode { get; }

        public String Source { get; set; }

        public OutputLevel OutputLevel { get; set; } = OutputLevel.Error;
    }
}