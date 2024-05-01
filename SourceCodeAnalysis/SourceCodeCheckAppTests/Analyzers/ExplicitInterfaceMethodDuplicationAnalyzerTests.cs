using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class ExplicitInterfaceMethodDuplicationAnalyzerTests
    {
        [Test]
        public void ProcessWithExplicitInterfaceMethodDuplicationWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(string p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void ISomeInterface.Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        void ISomeInterface.Do(string p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void Do(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(11): [ERROR]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(int)\r\n" +
                                                  "{0}(14): [ERROR]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(string)\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithExplicitInterfaceMethodDuplicationWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(string p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void ISomeInterface.Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        void ISomeInterface.Do(string p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void Do(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(11): [{1}]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(int)\r\n" +
                                                  "{0}(14): [{1}]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(string)\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithExplicitInterfaceMethodDuplicationWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(string p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void ISomeInterface.Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        void ISomeInterface.Do(string p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void Do(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {ExplicitInterfaceMethodDuplicationAnalyzer.Name} started\r\n" +
                                                  "Found 2 explicit implementations of an interface with a private method of the same name\r\n" +
                                                  "{0}(11): [{1}]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(int)\r\n" +
                                                  "{0}(14): [{1}]: Found explicit implementation of an interface with a private method of the same name: SomeNamespace.SomeClass.SomeNamespace.ISomeInterface.Do(string)\r\n" +
                                                  $"Execution of {ExplicitInterfaceMethodDuplicationAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithExplicitInterfaceMethodWithoutDuplication(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void ISomeInterface.Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void OtherDo(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithExplicitInterfaceMethodWithoutDuplicationWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void ISomeInterface.Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void OtherDo(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.ExplicitInterfaceMethodDuplicationAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.ExplicitInterfaceMethodDuplicationAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutExplicitInterfaceMethod(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        public void Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void Do(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutExplicitInterfaceMethodWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public interface ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        void Do(int p);\r\n" +
                                  "        void Do(int p1, string p2);\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass : ISomeInterface\r\n" +
                                  "    {\r\n" +
                                  "        public void Do(int p)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void Do(int p1, string p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        private void Do(int p1, int p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExplicitInterfaceMethodDuplication", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.ExplicitInterfaceMethodDuplicationAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.ExplicitInterfaceMethodDuplicationAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new ExplicitInterfaceMethodDuplicationAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new ExplicitInterfaceMethodDuplicationAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new ExplicitInterfaceMethodDuplicationAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
