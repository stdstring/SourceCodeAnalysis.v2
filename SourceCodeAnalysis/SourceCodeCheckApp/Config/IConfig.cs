namespace SourceCodeCheckApp.Config
{
    public interface IConfig
    {
        ConfigData LoadDefault();

        ConfigData Load(String projectName);
    }
}
