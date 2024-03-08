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
                case 3:
                    return ParseArgsImpl(args);
                default:
                    return new AppArgs(AppUsageMode.BadAppUsage);
            }
        }

        private static AppArgs ParseArgsImpl(String[] args)
        {
            if (args.Length == 1 && String.Equals(args[0], HelpOption))
                return new AppArgs(AppUsageMode.Help);
            if (args.Length == 1 && String.Equals(args[0], VersionOption))
                return new AppArgs(AppUsageMode.Version);
            ISet<String> parsedOptions = new HashSet<String>();
            AppArgs appArgs = new AppArgs(AppUsageMode.Analysis);
            foreach (String arg in args)
            {
                if (arg.StartsWith(SourceOption))
                {
                    if (!parsedOptions.Add(SourceOption))
                        return new AppArgs(AppUsageMode.BadSource);
                    String source = EnvironmentVariableHelper.ExpandEnvironmentVariables(arg.Substring(SourceOption.Length));
                    if (String.IsNullOrEmpty(source))
                        return new AppArgs(AppUsageMode.BadSource);
                    appArgs.Source = source;
                }
                else if (arg.StartsWith(ConfigOption))
                {
                    if (!parsedOptions.Add(ConfigOption))
                        return new AppArgs(AppUsageMode.BadConfig);
                    String config = EnvironmentVariableHelper.ExpandEnvironmentVariables(arg.Substring(ConfigOption.Length));
                    if (String.IsNullOrEmpty(config))
                        return new AppArgs(AppUsageMode.BadConfig);
                    appArgs.Config = config;
                }
                else if (arg.StartsWith(OutputLevelOption))
                {
                    if (!parsedOptions.Add(OutputLevelOption))
                        return new AppArgs(AppUsageMode.BadAppUsage);
                    if (!Enum.TryParse(arg.Substring(OutputLevelOption.Length), out OutputLevel outputLevel))
                        return new AppArgs(AppUsageMode.BadAppUsage);
                    appArgs.OutputLevel = outputLevel;
                }
                else
                    return new AppArgs(AppUsageMode.BadAppUsage);
            }
            return String.IsNullOrEmpty(appArgs.Source) ? new AppArgs(AppUsageMode.BadAppUsage) : appArgs;
        }

        private const String HelpOption = "--help";
        private const String VersionOption = "--version";
        private const String SourceOption = "--source=";
        private const String ConfigOption = "--config=";
        private const String OutputLevelOption = "--output-level=";
    }
}