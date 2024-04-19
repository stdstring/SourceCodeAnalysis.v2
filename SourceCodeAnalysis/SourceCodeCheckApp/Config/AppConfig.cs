using System.Xml.Serialization;
using SourceCodeCheckApp.Processors;

namespace SourceCodeCheckApp.Config
{
    internal record AppConfig(ConfigData Config);

    internal static class AppConfigFactory
    {
        public static AppConfig Create(AppArgsResult.MainConfig config)
        {
            if (!File.Exists(config.ConfigPath))
                throw new InvalidOperationException("Unknown config");
            using (StreamReader reader = new StreamReader(config.ConfigPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
                ConfigData? configData = serializer.Deserialize(reader) as ConfigData;
                if (configData == null)
                    throw new InvalidOperationException("Bad config data");
                (Boolean Success, String Reason) checkResult = ConfigDataChecker.Check(configData);
                if (!checkResult.Success)
                    throw new InvalidOperationException(checkResult.Reason);
                return new AppConfig(configData);
            }
        }
    }

    internal static class ConfigDataChecker
    {
        public static (Boolean Success, String Reason) Check(ConfigData config)
        {
            if (config.BaseConfig == null)
                return (false, "Bad Config.BaseConfig");
            if (config.BaseConfig.Source == null)
                return (false, "Bad Config.BaseConfig.Source");
            if (!File.Exists(config.BaseConfig.Source))
                return (false, "Unknown Config.BaseConfig.Source");
            if (!SourceProcessorFactory.IsSupportedSource(config.BaseConfig.Source))
                return (false, "Unsupported Config.BaseConfig.Source");
            if ((config.Analyzers != null) && config.Analyzers.Any(entry => String.IsNullOrEmpty(entry.Name)))
                return (false, "Bad Config.Analyzers entries");
            return (true, "");
        }
    }
}
