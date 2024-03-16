using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Args
{
    internal static class AppArgsParser
    {
        public static AppArgs Parse(String[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return new AppArgs(AppUsageMode.Help);
                case 1:
                case 2:
                    return ParseArgsImpl(args);
                default:
                    return new AppArgs(AppUsageMode.BadAppUsage);
            }
        }

        private static AppArgs ParseArgsImpl(String[] args)
        {
            switch (args)
            {
                case []:
                case [HelpOption]:
                    return new AppArgs(AppUsageMode.Help);
                case [VersionOption]:
                    return new AppArgs(AppUsageMode.Version);
                case [var arg0] when arg0.StartsWith(SourceOption):
                    return ParseAnalysisArgs(arg0.Substring(SourceOption.Length), "Error");
                case [var arg0, var arg1] when arg0.StartsWith(SourceOption) && arg1.StartsWith(OutputLevelOption):
                    return ParseAnalysisArgs(arg0.Substring(SourceOption.Length), arg1.Substring(OutputLevelOption.Length));
                case [var arg0, var arg1] when arg0.StartsWith(OutputLevelOption) && arg1.StartsWith(SourceOption):
                    return ParseAnalysisArgs(arg1.Substring(SourceOption.Length), arg0.Substring(OutputLevelOption.Length));
                default:
                    return new AppArgs(AppUsageMode.BadAppUsage);
            }
        }

        private static AppArgs ParseAnalysisArgs(String sourceValue, String outputLevelValue)
        {
            AppArgs appArgs = new AppArgs(AppUsageMode.Analysis);
            String source = EnvironmentVariableHelper.ExpandEnvironmentVariables(sourceValue);
            if (String.IsNullOrEmpty(source))
                return new AppArgs(AppUsageMode.BadSource);
            appArgs.Source = source;
            if (!Enum.TryParse(outputLevelValue, out OutputLevel outputLevel))
                return new AppArgs(AppUsageMode.BadAppUsage);
            appArgs.OutputLevel = outputLevel;
            return appArgs;
        }

        private const String HelpOption = "--help";
        private const String VersionOption = "--version";
        private const String SourceOption = "--source=";
        private const String OutputLevelOption = "--output-level=";
    }
}