namespace SourceCodeCheckApp.Config
{
    internal class EmptyConfig : IConfig
    {
        public ConfigData LoadDefault()
        {
            return new ConfigData();
        }

        public ConfigData Load(String projectName)
        {
            return new ConfigData();
        }
    }
}
