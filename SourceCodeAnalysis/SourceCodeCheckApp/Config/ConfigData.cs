namespace SourceCodeCheckApp.Config
{
    public class AttributeData
    {
        public AttributeData(String name, IDictionary<String, String> data)
        {
            Name = name;
            Data = data;
        }

        public String Name { get; }

        public IDictionary<String, String> Data { get; }
    }

    public enum FileProcessingMode
    {
        Include = 0,
        Exclude = 1,
        Only = 2
    }

    public class FileProcessingData
    {
        public FileProcessingData(FileProcessingMode mode, String mask)
        {
            Mode = mode;
            Mask = mask;
        }

        public FileProcessingMode Mode { get; }

        public String Mask { get; }
    }

    public class ConfigData
    {
        public ConfigData()
        {
            Attributes = new List<AttributeData>();
            FileProcessing = new List<FileProcessingData>();
        }

        public ConfigData(IList<AttributeData> attributes, IList<FileProcessingData> fileProcessing)
        {
            Attributes = attributes;
            FileProcessing = fileProcessing;
        }

        public IList<AttributeData> Attributes { get; }

        public IList<FileProcessingData> FileProcessing { get; }

        public static ConfigData Merge(ConfigData mainConfig, params ConfigData[] configs)
        {
            List<AttributeData> attributes = new List<AttributeData>();
            List<FileProcessingData> fileProcess = new List<FileProcessingData>();
            attributes.AddRange(mainConfig.Attributes);
            fileProcess.AddRange(mainConfig.FileProcessing);
            foreach (ConfigData config in configs)
            {
                attributes.AddRange(config.Attributes);
                fileProcess.AddRange(config.FileProcessing);
            }
            return new ConfigData(attributes, fileProcess);
        }
    }
}