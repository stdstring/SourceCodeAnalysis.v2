using SourceCodeCheckApp.Args;

namespace SourceCodeCheckApp.Config
{
    internal static class ConfigFactory
    {
        public static IConfig Create(AppArgs appArgs)
        {
            if (appArgs == null)
                throw new ArgumentNullException(nameof(appArgs));
            if (String.IsNullOrEmpty(appArgs.Config))
                return new EmptyConfig();
            String configPath = PorterConfigPathProvider.GetConfigPath(appArgs);
            return String.IsNullOrEmpty(configPath) ? null : new PorterConfig(configPath);
        }
    }
}
