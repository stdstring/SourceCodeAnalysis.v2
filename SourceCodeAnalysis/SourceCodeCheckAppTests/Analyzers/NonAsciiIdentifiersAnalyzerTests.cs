using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class NonAsciiIdentifiersAnalyzerTests
    {
        [Test]
        public void ProcessNonAsciiIdentifiersWithErrorLevel()
        {
            const String source = "namespace SomeНеймспейс\r\n" +
                                  "{\r\n" +
                                  "    public class ДругойClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeКласс\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeМетод()\r\n" +
                                  "        {\r\n" +
                                  "            int intПеременная = 666;\r\n" +
                                  "            string строковаяVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            ДругойClass другойObj = new ДругойClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = "{0}(1): [ERROR]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                                  "{0}(3): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(6): [ERROR]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                                  "{0}(8): [ERROR]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                                  "{0}(10): [ERROR]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                                  "{0}(11): [ERROR]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NonAsciiIdentifiers", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNonAsciiIdentifiersWithWarningLevel()
        {
            const String source = "namespace SomeНеймспейс\r\n" +
                                  "{\r\n" +
                                  "    public class ДругойClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeКласс\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeМетод()\r\n" +
                                  "        {\r\n" +
                                  "            int intПеременная = 666;\r\n" +
                                  "            string строковаяVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            ДругойClass другойObj = new ДругойClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = "{0}(1): [{1}]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                                  "{0}(3): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(6): [{1}]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                                  "{0}(8): [{1}]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                                  "{0}(10): [{1}]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                                  "{0}(11): [{1}]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NonAsciiIdentifiers", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNonAsciiIdentifiersWithInfoLevel()
        {
            const String source = "namespace SomeНеймспейс\r\n" +
                                  "{\r\n" +
                                  "    public class ДругойClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeКласс\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeМетод()\r\n" +
                                  "        {\r\n" +
                                  "            int intПеременная = 666;\r\n" +
                                  "            string строковаяVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            ДругойClass другойObj = new ДругойClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = $"Execution of {NonAsciiIdentifiersAnalyzer.Name} started\r\n" +
                                                  "Found 9 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                  "{0}(1): [{1}]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                                  "{0}(3): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(6): [{1}]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                                  "{0}(8): [{1}]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                                  "{0}(10): [{1}]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                                  "{0}(11): [{1}]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                                  "{0}(13): [{1}]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  $"Execution of {NonAsciiIdentifiersAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NonAsciiIdentifiers", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessAsciiIdentifiers(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int intVar = 666;\r\n" +
                                  "            string strVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            OtherClass otherObj = new OtherClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NonAsciiIdentifiers", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAsciiIdentifiersWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class OtherClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int intVar = 666;\r\n" +
                                  "            string strVar1 = \"IDDQD\";\r\n" +
                                  "            string stringVar2 = \"ИДДКуД\";\r\n" +
                                  "            OtherClass otherObj = new OtherClass();" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NonAsciiIdentifiers", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new NonAsciiIdentifiersAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new NonAsciiIdentifiersAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new NonAsciiIdentifiersAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
