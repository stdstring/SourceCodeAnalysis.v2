using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class NameOfExprAnalyzerTests
    {
        [Test]
        public void ProcessWithNameOfExprWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string p = \"IDDQD\";\r\n" +
                                  "            string p2 = GetValue(p);\r\n" +
                                  "            string p3 = nameof(SomeClass);\r\n" +
                                  "            string p4 = nameof(SomeMethod);\r\n" +
                                  "            string p5 = nameof(p);\r\n" +
                                  "        }\r\n" +
                                  "        public string GetValue(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"={0}=\", p);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [ERROR]: Found nameof expression: nameof(SomeClass)\r\n" +
                                                  "{0}(10): [ERROR]: Found nameof expression: nameof(SomeMethod)\r\n" +
                                                  "{0}(11): [ERROR]: Found nameof expression: nameof(p)\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithNameOfExprWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string p = \"IDDQD\";\r\n" +
                                  "            string p2 = GetValue(p);\r\n" +
                                  "            string p3 = nameof(SomeClass);\r\n" +
                                  "            string p4 = nameof(SomeMethod);\r\n" +
                                  "            string p5 = nameof(p);\r\n" +
                                  "        }\r\n" +
                                  "        public string GetValue(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"={0}=\", p);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [{1}]: Found nameof expression: nameof(SomeClass)\r\n" +
                                                  "{0}(10): [{1}]: Found nameof expression: nameof(SomeMethod)\r\n" +
                                                  "{0}(11): [{1}]: Found nameof expression: nameof(p)\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithNameOfExprWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string p = \"IDDQD\";\r\n" +
                                  "            string p2 = GetValue(p);\r\n" +
                                  "            string p3 = nameof(SomeClass);\r\n" +
                                  "            string p4 = nameof(SomeMethod);\r\n" +
                                  "            string p5 = nameof(p);\r\n" +
                                  "        }\r\n" +
                                  "        public string GetValue(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"={0}=\", p);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {NameOfExprAnalyzer.Name} started\r\n" +
                                                  "Found 3 nameof expressions\r\n" +
                                                  "{0}(9): [{1}]: Found nameof expression: nameof(SomeClass)\r\n" +
                                                  "{0}(10): [{1}]: Found nameof expression: nameof(SomeMethod)\r\n" +
                                                  "{0}(11): [{1}]: Found nameof expression: nameof(p)\r\n" +
                                                  $"Execution of {NameOfExprAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutNameOfExpr(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string p = \"IDDQD\";\r\n" +
                                  "            string p2 = GetValue(p);\r\n" +
                                  "            string p3 = nameof(p);\r\n" +
                                  "            string p4 = nameof(p2, p3);\r\n" +
                                  "        }\r\n" +
                                  "        public string GetValue(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"={0}=\", p);\r\n" +
                                  "        }\r\n" +
                                  "        public string nameof(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"nameof({0})\", p);\r\n" +
                                  "        }\r\n" +
                                  "        public string nameof(string p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"nameof({0}, {1})\", p1, p2);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutNameOfExprWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string p = \"IDDQD\";\r\n" +
                                  "            string p2 = GetValue(p);\r\n" +
                                  "            string p3 = nameof(p);\r\n" +
                                  "            string p4 = nameof(p2, p3);\r\n" +
                                  "        }\r\n" +
                                  "        public string GetValue(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"={0}=\", p);\r\n" +
                                  "        }\r\n" +
                                  "        public string nameof(string p)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"nameof({0})\", p);\r\n" +
                                  "        }\r\n" +
                                  "        public string nameof(string p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "            return string.Format(\"nameof({0}, {1})\", p1, p2);\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.NameOfExprAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.NameOfExprAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(NameOfExprAnalyzer.Name, NameOfExprAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new NameOfExprAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new NameOfExprAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new NameOfExprAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
