using System.Xml.Serialization;

namespace SourceCodeCheckApp.Config
{
    public enum OutputLevel
    {
        [XmlEnum(Name = "Error")]
        Error = 0,
        [XmlEnum(Name = "Warning")]
        Warning = 1,
        [XmlEnum(Name = "Info")]
        Info = 2
    }

    [XmlRoot("BaseConfig")]
    public class BaseConfig
    {
        [XmlElement("Source")]
        public String? Source { get; set; }

        [XmlElement("OutputLevel")]
        public OutputLevel OutputLevel { get; set; } = OutputLevel.Error;
    }

    public enum AnalyzerState
    {
        [XmlEnum(Name = "Off")]
        Off = 0,
        [XmlEnum(Name = "On")]
        On = 1,
        [XmlEnum(Name = "ErrorAsWarning")]
        ErrorAsWarning = 2
    }

    [XmlRoot("Analyzer")]
    public class AnalyzerEntry
    {
        [XmlElement("Name")]
        public String? Name { get; set; }

        [XmlElement("State")]
        public AnalyzerState State { get; set; } = AnalyzerState.Off;
    }

    [XmlRoot("Config")]
    public class ConfigData
    {
        [XmlElement("BaseConfig")]
        public BaseConfig? BaseConfig { get; set; }

        [XmlArray("Analyzers")]
        [XmlArrayItem("Analyzer")]
        public AnalyzerEntry[]? Analyzers { get; set; }
    }
}
