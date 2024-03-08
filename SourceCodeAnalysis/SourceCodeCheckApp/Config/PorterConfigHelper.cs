using System.Xml.Linq;

namespace SourceCodeCheckApp.Config
{
    internal static class PorterConfigHelper
    {
        public static IList<String> GetImports(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "import"))
                .Select(element => element.Attribute("config").Value)
                .Where(importFilename => !IsIgnoredImport(importFilename))
                .ToList();
        }

        public static IList<AttributeData> GetAttributes(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "attribute"))
                .Select(CreateAttributeData)
                .ToList();
        }

        public static IList<FileProcessingData> GetFileProcessing(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "files"))
                .SelectMany(element => element.Elements())
                .Select(CreateFileProcessingData)
                .ToList();
        }

        private static Boolean IsIgnoredImport(String importFilename)
        {
            return IgnoredImports.Any(importFilename.EndsWith);
        }

        private static AttributeData CreateAttributeData(XElement attributeElement)
        {
            String name = attributeElement.Attribute("name").Value;
            IDictionary<String, String> data = attributeElement.Attributes()
                .Where(attr => !String.Equals(attr.Name.LocalName, "name"))
                .ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
            return new AttributeData(name, data);
        }

        private static FileProcessingData CreateFileProcessingData(XElement fileProcessingElement)
        {
            const String includeElement = "include";
            const String excludeElement = "exclude";
            const String onlyElement = "only";
            IDictionary<String, FileProcessingMode> fileProcessModeMap = new Dictionary<String, FileProcessingMode>
            {
                {includeElement, FileProcessingMode.Include},
                {excludeElement, FileProcessingMode.Exclude},
                {onlyElement, FileProcessingMode.Only}
            };
            const String maskAttribute = "file";
            FileProcessingMode mode = fileProcessModeMap[fileProcessingElement.Name.LocalName];
            String mask = fileProcessingElement.Attribute(maskAttribute).Value;
            return new FileProcessingData(mode, mask);
        }

        private static readonly String[] IgnoredImports = {"include_map.config"};
    }
}