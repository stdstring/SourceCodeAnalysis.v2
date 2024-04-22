using System.Xml.Serialization;
using SourceCodeCheckApp.Config;

namespace SourceCodeCheckAppTests.Utils;

internal static class ConfigGenerator
{
    public static string Generate(string configPrefix, string source)
    {
        return Generate(configPrefix, source, OutputLevel.Error, new Dictionary<string, AnalyzerState>());
    }

    public static string Generate(string configPrefix, string source, OutputLevel outputLevel)
    {
        return Generate(configPrefix, source, outputLevel, new Dictionary<string, AnalyzerState>());
    }

    public static string Generate(string configPrefix, string source, OutputLevel outputLevel, IDictionary<string, AnalyzerState> analyzers)
    {
        ConfigData configData = new ConfigData
        {
            BaseConfig = new BaseConfig { Source = source, OutputLevel = outputLevel },
            Analyzers = analyzers.Select(kvPair => new AnalyzerEntry { Name = kvPair.Key, State = kvPair.Value }).ToArray()
        };
        DateTime now = DateTime.Now;
        string timePart = $"{now.Year}{now.Month:00}{now.Day:00}.{now.Hour:00}{now.Minute:00}{now.Second:00}{now.Millisecond:000}";
        string filePath = Path.GetFullPath($"./{configPrefix}.{timePart}{ConfigSuffix}");
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            serializer.Serialize(writer, configData);
        }
        return filePath;
    }

    public const string ConfigSuffix = ".testconfig.xml";
}