using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class OutInlineVariableAnalyzerTests
    {
        [Test]
        public void ProcessOutInlineVariableWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int p1 = 0;\r\n" +
                                  "            OtherMethod(out p1, out int p2, 5 * 6 + 1);\r\n" +
                                  "            bool result = int.TryParse(\"666\", out int value);\r\n" +
                                  "            int dest = p1 + p2 + value;\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(out int p1, out int p2, int p3)\r\n" +
                                  "        {\r\n" +
                                  "            p1 = 666 + p3;\r\n" +
                                  "            p2 = 777 + p3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [ERROR]: Found out inline variable when call: SomeNamespace.SomeClass.OtherMethod(out int, out int, int)\r\n" +
                                                  "{0}(9): [ERROR]: Found out inline variable when call: int.TryParse(string?, out int)\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "OutInlineVariable", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessOutInlineVariableWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int p1 = 0;\r\n" +
                                  "            OtherMethod(out p1, out int p2, 5 * 6 + 1);\r\n" +
                                  "            bool result = int.TryParse(\"666\", out int value);\r\n" +
                                  "            int dest = p1 + p2 + value;\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(out int p1, out int p2, int p3)\r\n" +
                                  "        {\r\n" +
                                  "            p1 = 666 + p3;\r\n" +
                                  "            p2 = 777 + p3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [{1}]: Found out inline variable when call: SomeNamespace.SomeClass.OtherMethod(out int, out int, int)\r\n" +
                                                  "{0}(9): [{1}]: Found out inline variable when call: int.TryParse(string?, out int)\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "OutInlineVariable", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessOutInlineVariableWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int p1 = 0;\r\n" +
                                  "            OtherMethod(out p1, out int p2, 5 * 6 + 1);\r\n" +
                                  "            bool result = int.TryParse(\"666\", out int value);\r\n" +
                                  "            int dest = p1 + p2 + value;\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(out int p1, out int p2, int p3)\r\n" +
                                  "        {\r\n" +
                                  "            p1 = 666 + p3;\r\n" +
                                  "            p2 = 777 + p3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {OutInlineVariableAnalyzer.Name} started\r\n" +
                                                  "Found 2 out inline variables\r\n" +
                                                  "{0}(8): [{1}]: Found out inline variable when call: SomeNamespace.SomeClass.OtherMethod(out int, out int, int)\r\n" +
                                                  "{0}(9): [{1}]: Found out inline variable when call: int.TryParse(string?, out int)\r\n" +
                                                  $"Execution of {OutInlineVariableAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "OutInlineVariable", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutOutInlineVariable(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int p1 = 0;\r\n" +
                                  "            int p2 = 0;\r\n" +
                                  "            OtherMethod(out p1, out p2, 5 * 6 + 1);\r\n" +
                                  "            int value = 0;\r\n" +
                                  "            bool result = int.TryParse(\"666\", out value);\r\n" +
                                  "            int dest = p1 + p2 + value;\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(out int p1, out int p2, int p3)\r\n" +
                                  "        {\r\n" +
                                  "            p1 = 666 + p3;\r\n" +
                                  "            p2 = 777 + p3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "OutInlineVariable", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutOutInlineVariableWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int p1 = 0;\r\n" +
                                  "            int p2 = 0;\r\n" +
                                  "            OtherMethod(out p1, out p2, 5 * 6 + 1);\r\n" +
                                  "            int value = 0;\r\n" +
                                  "            bool result = int.TryParse(\"666\", out value);\r\n" +
                                  "            int dest = p1 + p2 + value;\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(out int p1, out int p2, int p3)\r\n" +
                                  "        {\r\n" +
                                  "            p1 = 666 + p3;\r\n" +
                                  "            p2 = 777 + p3;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "OutInlineVariable", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.OutInlineVariableAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.OutInlineVariableAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new OutInlineVariableAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new OutInlineVariableAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new OutInlineVariableAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
