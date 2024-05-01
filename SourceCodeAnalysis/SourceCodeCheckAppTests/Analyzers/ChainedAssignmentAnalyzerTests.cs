using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class ChainedAssignmentAnalyzerTests
    {
        [Test]
        public void ProcessChainedAssignmentWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int a = 13;\r\n" +
                                  "            int b = 666;\r\n" +
                                  "            int c = a = b;\r\n" +
                                  "            a = b += c = a += b;\r\n" +
                                  "            c += b = a;\r\n" +
                                  "            OtherMethod(a = b = c);\r\n" +
                                  "            OtherMethod(a += b += c);\r\n" +
                                  "            for (int i = 0; i < 10; i += a += b)\r\n" +
                                  "            {\r\n" +
                                  "            }\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [ERROR]: Found chained assignments\r\n" +
                                                  "{0}(10): [ERROR]: Found chained assignments\r\n" +
                                                  "{0}(11): [ERROR]: Found chained assignments\r\n" +
                                                  "{0}(12): [ERROR]: Found chained assignments\r\n" +
                                                  "{0}(13): [ERROR]: Found chained assignments\r\n" +
                                                  "{0}(14): [ERROR]: Found chained assignments\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ChainedAssignment", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessChainedAssignmentWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int a = 13;\r\n" +
                                  "            int b = 666;\r\n" +
                                  "            int c = a = b;\r\n" +
                                  "            a = b += c = a += b;\r\n" +
                                  "            c += b = a;\r\n" +
                                  "            OtherMethod(a = b = c);\r\n" +
                                  "            OtherMethod(a += b += c);\r\n" +
                                  "            for (int i = 0; i < 10; i += a += b)\r\n" +
                                  "            {\r\n" +
                                  "            }\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(10): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(11): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(12): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(13): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(14): [{1}]: Found chained assignments\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ChainedAssignment", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessChainedAssignmentWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int a = 13;\r\n" +
                                  "            int b = 666;\r\n" +
                                  "            int c = a = b;\r\n" +
                                  "            a = b += c = a += b;\r\n" +
                                  "            c += b = a;\r\n" +
                                  "            OtherMethod(a = b = c);\r\n" +
                                  "            OtherMethod(a += b += c);\r\n" +
                                  "            for (int i = 0; i < 10; i += a += b)\r\n" +
                                  "            {\r\n" +
                                  "            }\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {ChainedAssignmentAnalyzer.Name} started\r\n" +
                                                  "Found 6 chained assignments\r\n" +
                                                  "{0}(9): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(10): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(11): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(12): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(13): [{1}]: Found chained assignments\r\n" +
                                                  "{0}(14): [{1}]: Found chained assignments\r\n" +
                                                  $"Execution of {ChainedAssignmentAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ChainedAssignment", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutChainedAssignment(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int a = 13;\r\n" +
                                  "            int b = 666;\r\n" +
                                  "            a = b;\r\n" +
                                  "            b += 77;\r\n" +
                                  "            a += b;\r\n" +
                                  "            OtherMethod(a = b);\r\n" +
                                  "            OtherMethod(a += b);\r\n" +
                                  "            for (int i = 0; i < 10; i += a)\r\n" +
                                  "            {\r\n" +
                                  "            }\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ChainedAssignment", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutChainedAssignmentWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int a = 13;\r\n" +
                                  "            int b = 666;\r\n" +
                                  "            a = b;\r\n" +
                                  "            b += 77;\r\n" +
                                  "            a += b;\r\n" +
                                  "            OtherMethod(a = b);\r\n" +
                                  "            OtherMethod(a += b);\r\n" +
                                  "            for (int i = 0; i < 10; i += a)\r\n" +
                                  "            {\r\n" +
                                  "            }\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ChainedAssignment", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.ChainedAssignmentAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.ChainedAssignmentAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new ChainedAssignmentAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new ChainedAssignmentAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new ChainedAssignmentAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
