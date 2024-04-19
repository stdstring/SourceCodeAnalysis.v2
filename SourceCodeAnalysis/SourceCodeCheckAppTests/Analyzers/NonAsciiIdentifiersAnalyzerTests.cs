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
        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessNonAsciiIdentifiers(OutputLevel outputLevel)
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(1): [ERROR]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                                  "{0}(3): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(6): [ERROR]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                                  "{0}(8): [ERROR]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                                  "{0}(10): [ERROR]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                                  "{0}(11): [ERROR]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "NonAsciiIdentifiers", filePath, outputLevel, false, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                                  "Found 9 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                  "{0}(1): [ERROR]: Found non-ASCII identifier \"SomeНеймспейс\"\r\n" +
                                                  "{0}(3): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(6): [ERROR]: Found non-ASCII identifier \"SomeКласс\"\r\n" +
                                                  "{0}(8): [ERROR]: Found non-ASCII identifier \"SomeМетод\"\r\n" +
                                                  "{0}(10): [ERROR]: Found non-ASCII identifier \"intПеременная\"\r\n" +
                                                  "{0}(11): [ERROR]: Found non-ASCII identifier \"строковаяVar1\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"другойObj\"\r\n" +
                                                  "{0}(13): [ERROR]: Found non-ASCII identifier \"ДругойClass\"\r\n" +
                                                  "Execution of NonAsciiIdentifiersAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "NonAsciiIdentifiers", filePath, OutputLevel.Info, false, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "NonAsciiIdentifiers", filePath, outputLevel, true, "");
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "NonAsciiIdentifiers", filePath, OutputLevel.Info, true, SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput);
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerFactory = output => new NonAsciiIdentifiersAnalyzer(output, AnalyzerState.On);
    }
}
