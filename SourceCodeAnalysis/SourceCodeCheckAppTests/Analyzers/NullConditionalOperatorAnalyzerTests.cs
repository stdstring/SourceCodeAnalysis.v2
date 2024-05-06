using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    internal class NullConditionalOperatorAnalyzerTests
    {
        [Test]
        public void ProcessNullConditionalOperatorWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass? obj1 = new SomeClass();\r\n" +
                                  "            SomeClass? obj2 = null;\r\n" +
                                  "            SomeClass[] objects = null;\r\n" +
                                  "            obj1?.OtherMethod();\r\n" +
                                  "            obj2?.OtherMethod();\r\n" +
                                  "            objects?[0].OtherMethod();\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(10): [ERROR]: Found null-conditional operator\r\n" +
                                                  "{0}(11): [ERROR]: Found null-conditional operator\r\n" +
                                                  "{0}(12): [ERROR]: Found null-conditional operator\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullConditionalOperator", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNullConditionalOperatorWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass? obj1 = new SomeClass();\r\n" +
                                  "            SomeClass? obj2 = null;\r\n" +
                                  "            SomeClass[] objects = null;\r\n" +
                                  "            obj1?.OtherMethod();\r\n" +
                                  "            obj2?.OtherMethod();\r\n" +
                                  "            objects?[0].OtherMethod();\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(10): [{1}]: Found null-conditional operator\r\n" +
                                                  "{0}(11): [{1}]: Found null-conditional operator\r\n" +
                                                  "{0}(12): [{1}]: Found null-conditional operator\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullConditionalOperator", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessNullConditionalOperatorWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass? obj1 = new SomeClass();\r\n" +
                                  "            SomeClass? obj2 = null;\r\n" +
                                  "            SomeClass[] objects = null;\r\n" +
                                  "            obj1?.OtherMethod();\r\n" +
                                  "            obj2?.OtherMethod();\r\n" +
                                  "            objects?[0].OtherMethod();\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {NullConditionalOperatorAnalyzer.Name} started\r\n" +
                                                  "Found 3 null-conditional operators\r\n" +
                                                  "{0}(10): [{1}]: Found null-conditional operator\r\n" +
                                                  "{0}(11): [{1}]: Found null-conditional operator\r\n" +
                                                  "{0}(12): [{1}]: Found null-conditional operator\r\n" +
                                                  $"Execution of {NullConditionalOperatorAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullConditionalOperator", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutNullConditionalOperator(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass obj = new SomeClass();\r\n" +
                                  "            SomeClass[] objects = new SomeClass[1];\r\n" +
                                  "            objects[0] = obj;\r\n" +
                                  "            obj.OtherMethod();\r\n" +
                                  "            objects[0].OtherMethod();\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullConditionalOperator", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutNullConditionalOperatorWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass obj = new SomeClass();\r\n" +
                                  "            SomeClass[] objects = new SomeClass[1];\r\n" +
                                  "            objects[0] = obj;\r\n" +
                                  "            obj.OtherMethod();\r\n" +
                                  "            objects[0].OtherMethod();\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "NullConditionalOperator", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.NullConditionalOperatorAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.NullConditionalOperatorAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(NullConditionalOperatorAnalyzer.Name, NullConditionalOperatorAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new NullConditionalOperatorAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new NullConditionalOperatorAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new NullConditionalOperatorAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
