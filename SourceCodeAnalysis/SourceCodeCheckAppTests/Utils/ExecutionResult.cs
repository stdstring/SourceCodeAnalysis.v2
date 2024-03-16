namespace SourceCodeCheckAppTests.Utils
{
    internal class ExecutionResult
    {
        public ExecutionResult(Int32 exitCode, String outputData, String errorData)
        {
            ExitCode = exitCode;
            OutputData = outputData;
            ErrorData = errorData;
        }

        public Int32 ExitCode { get; }

        public String OutputData { get; }

        public String ErrorData { get; }
    }
}