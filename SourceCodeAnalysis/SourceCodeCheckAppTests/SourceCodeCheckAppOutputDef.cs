using SourceCodeCheckApp.Analyzers;

namespace SourceCodeCheckAppTests
{
    internal static class SourceCodeCheckAppOutputDef
    {
        public const String BadArgsMessage = "[ERROR]: Bad args";
        public const String UnknownConfigMessage = "[ERROR]: Unknown config";
        public const String BadConfigMessage = "[ERROR]: Bad config path";
        public const String UnknownSourceMessage = "[ERROR]: Unknown Config.BaseConfig.Source";
        public const String UnsupportedSourceMessage = "[ERROR]: Unsupported Config.BaseConfig.Source";
        public const String AppDescription = "Application usage:\r\n" +
                                             "1. <app> --config=<path to config file>\r\n" +
                                             "2. <app> --help\r\n" +
                                             "3. <app> --version\r\n";

        public const String CompilationCheckSuccessOutput = "Checking compilation for errors and warnings:\r\n" +
                                                            "Found 0 errors in the compilation\r\n" +
                                                            "Found 0 warnings in the compilation\r\n";

        public const String BadFilenameCaseAnalyzerSuccessOutput = $"Execution of {BadFilenameCaseAnalyzer.Name} started\r\n" +
                                                                   "File contains 0 types with names match to the filename with ignoring case\r\n" +
                                                                   $"Execution of {BadFilenameCaseAnalyzer.Name} finished\r\n";

        public const String CastToSameTypeAnalyzerSuccessOutput = $"Execution of {CastToSameTypeAnalyzer.Name} started\r\n" +
                                                                  "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                                                  "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                                  $"Execution of {CastToSameTypeAnalyzer.Name} finished\r\n";

        public const String NonAsciiIdentifiersAnalyzerSuccessOutput = $"Execution of {NonAsciiIdentifiersAnalyzer.Name} started\r\n" +
                                                                       "Found 0 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                                       $"Execution of {NonAsciiIdentifiersAnalyzer.Name} finished\r\n";

        public const String StringInterpolationExprAnalyzerSuccessOutput = $"Execution of {StringInterpolationExprAnalyzer.Name} started\r\n" +
                                                                           "Found 0 string interpolation expressions\r\n" +
                                                                           $"Execution of {StringInterpolationExprAnalyzer.Name} finished\r\n";

        public const String DefaultLiteralAnalyzerSuccessOutput = $"Execution of {DefaultLiteralAnalyzer.Name} started\r\n" +
                                                                  "Found 0 target-typed default literals\r\n" +
                                                                  $"Execution of {DefaultLiteralAnalyzer.Name} finished\r\n";

        public const String ObjectInitializerExprAnalyzerSuccessOutput = $"Execution of {ObjectInitializerExprAnalyzer.Name} started\r\n" +
                                                                         "Found 0 object initializer expressions\r\n" +
                                                                         $"Execution of {ObjectInitializerExprAnalyzer.Name} finished\r\n";

        public const String AutoImplPropertiesAnalyzerSuccessOutput = $"Execution of {AutoImplPropertiesAnalyzer.Name} started\r\n" +
                                                                      "Found 0 auto implemented properties\r\n" +
                                                                      $"Execution of {AutoImplPropertiesAnalyzer.Name} finished\r\n";

        public const String ExprBodiedMemberAnalyzerSuccessOutput = $"Execution of {ExprBodiedMemberAnalyzer.Name} started\r\n" +
                                                                    "Found 0 expression-bodied members\r\n" +
                                                                    $"Execution of {ExprBodiedMemberAnalyzer.Name} finished\r\n";
    }
}