using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class ObjectInitializerExprAnalyzerTests
    {
        [Test]
        public void ProcessObjectInitializerExprWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeData d1 = new SomeData(){F1 = 999, F2 = \"IDKFA\"};\r\n" +
                                  "            SomeData d2 = new SomeData(777){F2 = \"IDKFA\"};\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(22): [ERROR]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [ERROR]: Found object initializer expressions\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessObjectInitializerExprWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeData d1 = new SomeData(){F1 = 999, F2 = \"IDKFA\"};\r\n" +
                                  "            SomeData d2 = new SomeData(777){F2 = \"IDKFA\"};\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(22): [{1}]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [{1}]: Found object initializer expressions\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessObjectInitializerExprWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeData d1 = new SomeData(){F1 = 999, F2 = \"IDKFA\"};\r\n" +
                                  "            SomeData d2 = new SomeData(777){F2 = \"IDKFA\"};\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {ObjectInitializerExprAnalyzer.Name} started\r\n" +
                                                  "Found 2 object initializer expressions\r\n" +
                                                  "{0}(22): [{1}]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [{1}]: Found object initializer expressions\r\n" +
                                                  $"Execution of {ObjectInitializerExprAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessObjectInitializerExprInMethodCallWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            OtherMethod(new SomeData(){F1 = 999, F2 = \"IDKFA\"});\r\n" +
                                  "            OtherMethod(new SomeData(777){F2 = \"IDKFA\"});\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(SomeData d)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(22): [ERROR]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [ERROR]: Found object initializer expressions\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessObjectInitializerExprInMethodCallWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            OtherMethod(new SomeData(){F1 = 999, F2 = \"IDKFA\"});\r\n" +
                                  "            OtherMethod(new SomeData(777){F2 = \"IDKFA\"});\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(SomeData d)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(22): [{1}]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [{1}]: Found object initializer expressions\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessObjectInitializerExprInMethodCallWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            OtherMethod(new SomeData(){F1 = 999, F2 = \"IDKFA\"});\r\n" +
                                  "            OtherMethod(new SomeData(777){F2 = \"IDKFA\"});\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod(SomeData d)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {ObjectInitializerExprAnalyzer.Name} started\r\n" +
                                                  "Found 2 object initializer expressions\r\n" +
                                                  "{0}(22): [{1}]: Found object initializer expressions\r\n" +
                                                  "{0}(23): [{1}]: Found object initializer expressions\r\n" +
                                                  $"Execution of {ObjectInitializerExprAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutObjectInitializerExpr(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeData d1 = new SomeData();\r\n" +
                                  "            d1.F1 = 999;\r\n" +
                                  "            d1.F2 = \"IDKFA\";\r\n" +
                                  "            SomeData d2 = new SomeData(777);\r\n" +
                                  "            d2.F2 = \"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutObjectInitializerExprWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeData\r\n" +
                                  "    {\r\n" +
                                  "        public SomeData()\r\n" +
                                  "        {\r\n" +
                                  "            F1 = 666;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public SomeData(int f1)\r\n" +
                                  "        {\r\n" +
                                  "            F1 = f1;\r\n" +
                                  "            F2 = \"IDDQD\";\r\n" +
                                  "        }\r\n" +
                                  "        public int F1;\r\n" +
                                  "        public string F2;\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeData d1 = new SomeData();\r\n" +
                                  "            d1.F1 = 999;\r\n" +
                                  "            d1.F2 = \"IDKFA\";\r\n" +
                                  "            SomeData d2 = new SomeData(777);\r\n" +
                                  "            d2.F2 = \"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ObjectInitializerExpr", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.ObjectInitializerExprAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.ObjectInitializerExprAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(ObjectInitializerExprAnalyzer.Name, ObjectInitializerExprAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new ObjectInitializerExprAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new ObjectInitializerExprAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new ObjectInitializerExprAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
