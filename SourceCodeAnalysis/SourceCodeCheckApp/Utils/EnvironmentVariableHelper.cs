namespace SourceCodeCheckApp.Utils
{
    internal static class EnvironmentVariableHelper
    {
        public static String ExpandEnvironmentVariables(String source)
        {
            String dest = Environment.ExpandEnvironmentVariables(source);
            return dest.IndexOf(EnvironmentVariableBorder) == -1 ? dest : String.Empty;
        }

        public const Char EnvironmentVariableBorder = '%';
    }
}