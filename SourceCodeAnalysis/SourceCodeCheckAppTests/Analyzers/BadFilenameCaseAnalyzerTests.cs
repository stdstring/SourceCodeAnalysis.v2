using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
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
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            const String expectedOutputTemplate = "{0}(9): [WARNING]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [WARNING]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            const String expectedOutputTemplate = $"Execution of {BadFilenameCaseAnalyzer.Name} started\r\n" +
                                                  "File contains 2 types with names match to the filename with ignoring case\r\n" +
                                                  "{0}(9): [WARNING]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [WARNING]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  $"Execution of {BadFilenameCaseAnalyzer.Name} finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutExactMatchWithErrorLevel()
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
            const String expectedOutputTemplate = "{0}(6): [ERROR]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(9): [ERROR]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [ERROR]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutExactMatchWithWarningLevel()
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
            const String expectedOutputTemplate = "{0}(6): [{1}]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(9): [{1}]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [{1}]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            const String expectedOutputTemplate = $"Execution of {BadFilenameCaseAnalyzer.Name} started\r\n" +
                                                  "File doesn't contain any type with name exact match to the filename, but contains 3 types with names match to the filename with ignoring case\r\n" +
                                                  "{0}(6): [{1}]: Found type named \"SomeNamespace.SoMeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(9): [{1}]: Found type named \"SomeNamespace.SOmeClass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  "{0}(12): [{1}]: Found type named \"SomeNamespace.Someclass\" which corresponds the filename \"SomeClass.cs\" only at ignoring case\r\n" +
                                                  $"Execution of {BadFilenameCaseAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            const String expectedOutputTemplate = "{0}(1): [WARNING]: File doesn't contain any types with names corresponding to the name of this file\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
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
            const String expectedOutputTemplate = $"Execution of {BadFilenameCaseAnalyzer.Name} started\r\n" +
                                                  "{0}(1): [WARNING]: File doesn't contain any types with names corresponding to the name of this file\r\n" +
                                                  $"Execution of {BadFilenameCaseAnalyzer.Name} finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "BadFilenameCase", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new BadFilenameCaseAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new BadFilenameCaseAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new BadFilenameCaseAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
