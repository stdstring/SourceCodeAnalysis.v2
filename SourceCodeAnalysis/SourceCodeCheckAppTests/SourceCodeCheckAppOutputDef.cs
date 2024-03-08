namespace SourceCodeCheckAppTests
{
    internal static class SourceCodeCheckAppOutputDef
    {
        public const String BadUsageMessage = "[ERROR]: Bad usage of the application.\r\n";
        public const String BadSourceMessage = "[ERROR]: Bad/empty/unknown source path.\r\n";
        public const String BadConfigMessage = "[ERROR]: Bad/empty/unknown config path.\r\n";
        public const String AppDescription = "Application usage:\r\n" +
                                             "1. {APP} --source={solution-filename.sln|project-filename.csproj|cs-filename.cs} [--config={config-file|config-dir}] [--output-level={Error|Warning|Info}]\r\n" +
                                             "2. {APP} --help\r\n" +
                                             "3. {APP} --version\r\n" +
                                             "Default values:\r\n" +
                                             "1. output-level=Error\r\n";

        public const String CompilationCheckSuccessOutput = "Checking compilation for errors and warnings:\r\n" +
                                                            "Found 0 errors in the compilation\r\n" +
                                                            "Found 0 warnings in the compilation\r\n";

        public const String BadFilenameCaseAnalyzerSuccessOutput = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                                   "File contains 0 types with names match to the filename with ignoring case\r\n" +
                                                                   "Execution of BadFilenameCaseAnalyzer finished\r\n";

        public const String CastToSameTypeAnalyzerSuccessOutput = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                                  "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                                                  "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                                  "Execution of CastToSameTypeAnalyzer finished\r\n";

        public const String NonAsciiIdentifiersAnalyzerSuccessOutput = "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                                                       "Found 0 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                                       "Execution of NonAsciiIdentifiersAnalyzer finished\r\n";
    }
}