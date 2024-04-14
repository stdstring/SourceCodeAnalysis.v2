namespace SourceCodeCheckAppTests
{
    internal static class SourceCodeCheckAppOutputDef
    {
        public const String BadArgsMessage = "[ERROR]: Bad args";
        public const String UnknownConfigMessage = "[ERROR]: Unknown config";
        public const String BadConfigMessage = "[ERROR]: Bad config path";
        public const String UnknownSourceMessage = "[ERROR]: Unknown Config.BaseConfig.Source";
        public const String AppDescription = "Application usage:\r\n" +
                                             "1. <app> --config=<path to config file>\r\n" +
                                             "2. <app> --help\r\n" +
                                             "3. <app> --version\r\n";

        public const String NugetRestoreOutput = "  Determining projects to restore...\r\n" +
                                                 "  All projects are up-to-date for restore.\r\n";

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