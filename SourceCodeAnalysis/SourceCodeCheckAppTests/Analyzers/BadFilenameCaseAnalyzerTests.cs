using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class BadFilenameCaseAnalyzerTests
    {
        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessExactMatch(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, outputLevel, true, "");
        }

        [Test]
        public void ProcessExactMatchWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Info, true, SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput);
        }

        [Test]
        public void ProcessExactMatchWithWarningsWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Error, true, "");
        }

        [Test]
        public void ProcessExactMatchWithWarningsWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(9): [WARNING]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [WARNING]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Warning, true, expectedOutput);
        }

        [Test]
        public void ProcessExactMatchWithWarningsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "File contains 2 types with names match to the filename with ignoring case\r\n" +
                                                  "{0}(9): [WARNING]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [WARNING]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Info, true, expectedOutput);
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutExactMatch(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SoMeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(6): [ERROR]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(9): [ERROR]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [ERROR]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, outputLevel, false, expectedOutput);
        }

        [Test]
        public void ProcessWithoutExactMatchWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SoMeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SOmeClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class Someclass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                          "File doesn't contain any type with name exact match to the filename, but contains 3 types with names match to the filename with ignoring case\r\n" +
                                          "{0}(6): [ERROR]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                          "{0}(9): [ERROR]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                          "{0}(12): [ERROR]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                          "Execution of BadFilenameCaseAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Info, false, expectedOutput);
        }

        [Test]
        public void ProcessWithoutMatchWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Error, true, "");
        }

        [Test]
        public void ProcessWithoutMatchWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(1): [WARNING]: File doesn't contain any types with names corresponding to the name of this file\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Warning, true, expectedOutput);
        }

        [Test]
        public void ProcessWithoutMatchWithVerbose()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class AnotherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "{0}(1): [WARNING]: File doesn't contain any types with names corresponding to the name of this file\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "BadFilenameCase", filePath, OutputLevel.Info, true, expectedOutput);
        }

        private readonly Func<OutputImpl, IFileAnalyzer> _analyzerFactory = output => new BadFilenameCaseAnalyzer(output);
    }
}
