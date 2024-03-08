using NUnit.Framework;

namespace SourceCodeCheckAppTests.Utils
{
    internal static class ExecutionChecker
    {
        public static void Check(ExecutionResult result, Int32 exitCode, String outputData, String errorData)
        {
            Assert.That(result, Is.Not.Null);
            Int32 actualExitCode = result.ExitCode;
            String actualOutputData = result.OutputData;
            String actualErrorData = result.ErrorData;
            if (exitCode != actualExitCode)
            {
                Console.WriteLine($"Expected exit code is {exitCode}, but actual exit code is {actualExitCode}");
                Console.WriteLine($"Actual output: {actualOutputData}");
                Console.WriteLine($"Actual error: {actualErrorData}");
            }
            Assert.That(actualExitCode, Is.EqualTo(exitCode));
            Assert.That(actualOutputData, Is.EqualTo(outputData));
            Assert.That(actualErrorData, Is.EqualTo(errorData));
        }
    }
}