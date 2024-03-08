using SourceCodeCheckApp.Args;

namespace SourceCodeCheckApp.Config
{
    internal static class PorterConfigPathProvider
    {
        public static String GetConfigPath(AppArgs appArgs)
        {
            if (appArgs == null)
                throw new ArgumentNullException(nameof(appArgs));
            return IsFile(appArgs.Config) || Directory.Exists(appArgs.Config) ? appArgs.Config : null;
        }

        public static String GetDefaultConfigPath(String configPath)
        {
            if (IsFile(configPath))
                return configPath;
            String configFilename = Path.Combine(configPath, DefaultConfigName);
            return IsFile(configFilename) ? configFilename : null;
        }

        public static String GetProjectConfigPath(String configPath, String projectName)
        {
            const String configExtension = ".config";
            String containedDirectory = IsFile(configPath) ? Path.GetDirectoryName(configPath) : configPath;
            String projectConfig = Path.Combine(containedDirectory ?? String.Empty, String.Concat(projectName, configExtension));
            return IsFile(projectConfig) ? projectConfig : null;
        }

        private static Boolean IsFile(String path)
        {
            return File.Exists(path);
        }

        public const String DefaultConfigName = "porter.config";
    }
}