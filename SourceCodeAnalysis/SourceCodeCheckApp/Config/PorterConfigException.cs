namespace SourceCodeCheckApp.Config
{
    public class PorterConfigException : Exception
    {
        public PorterConfigException()
        {
        }

        public PorterConfigException(String message) : base(message)
        {
        }

        public PorterConfigException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
