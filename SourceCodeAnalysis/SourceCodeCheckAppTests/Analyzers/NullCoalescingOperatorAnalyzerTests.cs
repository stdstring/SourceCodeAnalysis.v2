using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class NullCoalescingOperatorAnalyzerTests
    {
        [Test]
        public void ProcessNullCoalescingOperatorsWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string? s1 = null;\r\n" +
                                  "            string s2 = s1 ?? \"IDDQD\";\r\n" +
                                  "            s1 ??= \"IDKFA\";\r\n" +
                                  "            s2 ??= \"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [ERROR]: Found null-coalescing operator: \"??\"\r\n" +
                                                  "{0}(9): [ERROR]: Found null-coalescing operator: \"??=\"\r\n" +
                                                  "{0}(10): [ERROR]: Found null-coalescing operator: \"??=\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullCoalescingOperator", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNullCoalescingOperatorsWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string? s1 = null;\r\n" +
                                  "            string s2 = s1 ?? \"IDDQD\";\r\n" +
                                  "            s1 ??= \"IDKFA\";\r\n" +
                                  "            s2 ??= \"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [{1}]: Found null-coalescing operator: \"??\"\r\n" +
                                                  "{0}(9): [{1}]: Found null-coalescing operator: \"??=\"\r\n" +
                                                  "{0}(10): [{1}]: Found null-coalescing operator: \"??=\"\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullCoalescingOperator", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNullCoalescingOperatorsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string? s1 = null;\r\n" +
                                  "            string s2 = s1 ?? \"IDDQD\";\r\n" +
                                  "            s1 ??= \"IDKFA\";\r\n" +
                                  "            s2 ??= \"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {NullCoalescingOperatorAnalyzer.Name} started\r\n" +
                                                  "Found 3 null-coalescing operators\r\n" +
                                                  "{0}(8): [{1}]: Found null-coalescing operator: \"??\"\r\n" +
                                                  "{0}(9): [{1}]: Found null-coalescing operator: \"??=\"\r\n" +
                                                  "{0}(10): [{1}]: Found null-coalescing operator: \"??=\"\r\n" +
                                                  $"Execution of {NullCoalescingOperatorAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullCoalescingOperator", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutNullCoalescingOperators(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string? s1 = \"IDDQD\";\r\n" +
                                  "            string? s2 = \"IDKFA\";\r\n" +
                                  "            string s3 = s1 + s2;\r\n" +
                                  "            string s4 = s1 == null ? s2 : s1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullCoalescingOperator", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutNullCoalescingOperatorsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string? s1 = \"IDDQD\";\r\n" +
                                  "            string? s2 = \"IDKFA\";\r\n" +
                                  "            string s3 = s1 + s2;\r\n" +
                                  "            string s4 = s1 == null ? s2 : s1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullCoalescingOperator", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.NullCoalescingOperatorAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.NullCoalescingOperatorAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(NullCoalescingOperatorAnalyzer.Name, NullCoalescingOperatorAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new NullCoalescingOperatorAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new NullCoalescingOperatorAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new NullCoalescingOperatorAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
