using System.Xml.Linq;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Config
{
    internal class PorterConfig : IConfig
    {
        public PorterConfig(String configPath)
        {
            if (String.IsNullOrEmpty(configPath))
                throw new ArgumentNullException(nameof(configPath));
            _configPath = configPath;
            _configData = null;
        }

        public ConfigData LoadDefault()
        {
            if (_configData == null)
            {
                String configFilename = PorterConfigPathProvider.GetDefaultConfigPath(_configPath);
                if (String.IsNullOrEmpty(configFilename))
                    throw new FileNotFoundException();
                XDocument document = XDocument.Load(configFilename);
                _configData = LoadImpl(document.Root, configFilename);
            }
            return _configData;
        }

        public ConfigData Load(String projectName)
        {
            String projectConfigPath = PorterConfigPathProvider.GetProjectConfigPath(_configPath, projectName);
            if (!String.IsNullOrEmpty(projectConfigPath))
            {
                XDocument document = XDocument.Load(projectConfigPath);
                return LoadImpl(document.Root, projectConfigPath);
            }
            return LoadDefault();
        }

        // TODO (std_string) : think about non-recursive version of this impl
        private ConfigData LoadImpl(String sourceImportName, String parentConfig)
        {
            String importName = EnvironmentVariableHelper.ExpandEnvironmentVariables(sourceImportName).Replace("/", "\\");
            if (String.IsNullOrEmpty(importName))
                throw new PorterConfigException($"Bad import name value \"{sourceImportName}\" in the config \"{parentConfig}\"");
            // TODO (std_string) : we must know how to process case when importName will be relative to porter directory (if 'use_porter_home_directory_while_resolving_path' option is enabled)
            String importConfigName = Path.IsPathRooted(importName) ? importName : Path.Combine(Path.GetDirectoryName(parentConfig) ?? String.Empty, importName);
            XDocument document = XDocument.Load(importConfigName);
            return LoadImpl(document.Root, importConfigName);
        }

        // TODO (std_string) : think about non-recursive version of this impl
        private ConfigData LoadImpl(XElement root, String currentConfig)
        {
            IList<AttributeData> attributes = PorterConfigHelper.GetAttributes(root);
            IList<FileProcessingData> fileProcessing = PorterConfigHelper.GetFileProcessing(root);
            ConfigData[] configs = PorterConfigHelper.GetImports(root).Select(import => LoadImpl(import, currentConfig)).ToArray();
            return ConfigData.Merge(new ConfigData(attributes, fileProcessing), configs);
        }

        private readonly String _configPath;
        private ConfigData _configData;
    }
}
