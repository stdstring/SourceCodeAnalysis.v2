using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class AutoImplPropertiesAnalyzerTests
    {
        [Test]
        public void ProcessAutoImplPropertiesWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; }\r\n" +
                                  "        public int Prop2 { get; private set; }\r\n" +
                                  "        public int Prop3 { get; set; }\r\n" +
                                  "        public int Prop4 { get; init; }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [ERROR]: Found auto implemented property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(6): [ERROR]: Found auto implemented property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(7): [ERROR]: Found auto implemented property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(8): [ERROR]: Found auto implemented property: SomeNamespace.SomeClass.Prop4\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; }\r\n" +
                                  "        public int Prop2 { get; private set; }\r\n" +
                                  "        public int Prop3 { get; set; }\r\n" +
                                  "        public int Prop4 { get; init; }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(6): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(7): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(8): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop4\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; }\r\n" +
                                  "        public int Prop2 { get; private set; }\r\n" +
                                  "        public int Prop3 { get; set; }\r\n" +
                                  "        public int Prop4 { get; init; }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {AutoImplPropertiesAnalyzer.Name} started\r\n" +
                                                  "Found 4 auto implemented properties\r\n" +
                                                  "{0}(5): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(6): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(7): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(8): [{1}]: Found auto implemented property: SomeNamespace.SomeClass.Prop4\r\n" +
                                                  $"Execution of {AutoImplPropertiesAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInInnerClassWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get { return _field; } set { _field = value; } }\r\n" +
                                  "        private int _field;\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(10): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(11): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(12): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInInnerClassWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get { return _field; } set { _field = value; } }\r\n" +
                                  "        private int _field;\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(9): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(10): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(11): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(12): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInInnerClassWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get { return _field; } set { _field = value; } }\r\n" +
                                  "        private int _field;\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {AutoImplPropertiesAnalyzer.Name} started\r\n" +
                                                  "Found 4 auto implemented properties\r\n" +
                                                  "{0}(9): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(10): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(11): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(12): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n" +
                                                  $"Execution of {AutoImplPropertiesAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInOuterInnerClassesWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; set; }\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.Prop1\r\n" +
                                                  "{0}(8): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(9): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(10): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(11): [ERROR]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInOuterInnerClassesWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; set; }\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.Prop1\r\n" +
                                                  "{0}(8): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(9): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(10): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(11): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessAutoImplPropertiesInOuterInnerClassesWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeOuterClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get; set; }\r\n" +
                                  "        public class SomeInnerClass\r\n" +
                                  "        {\r\n" +
                                  "            public int Prop1 { get; }\r\n" +
                                  "            public int Prop2 { get; private set; }\r\n" +
                                  "            public int Prop3 { get; set; }\r\n" +
                                  "            public int Prop4 { get; init; }\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {AutoImplPropertiesAnalyzer.Name} started\r\n" +
                                                  "Found 5 auto implemented properties\r\n" +
                                                  "{0}(5): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.Prop1\r\n" +
                                                  "{0}(8): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop1\r\n" +
                                                  "{0}(9): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop2\r\n" +
                                                  "{0}(10): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop3\r\n" +
                                                  "{0}(11): [{1}]: Found auto implemented property: SomeNamespace.SomeOuterClass.SomeInnerClass.Prop4\r\n" +
                                                  $"Execution of {AutoImplPropertiesAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutAutoImplProperties(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get { return _field;} }\r\n" +
                                  "        public int Prop2 { set { _field = value; } }\r\n" +
                                  "        public int Prop3 { get { return _field; } set { _field = value; } }\r\n" +
                                  "        public int Prop4 { get { return _field; } init { _field = value; } }\r\n" +
                                  "        public int this[int index] { get { return _field; } set { _field = value; } }\r\n" +
                                  "        private int _field;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutAutoImplPropertiesWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public int Prop1 { get { return _field;} }\r\n" +
                                  "        public int Prop2 { set { _field = value; } }\r\n" +
                                  "        public int Prop3 { get { return _field; } set { _field = value; } }\r\n" +
                                  "        public int Prop4 { get { return _field; } init { _field = value; } }\r\n" +
                                  "        public int this[int index] { get { return _field; } set { _field = value; } }\r\n" +
                                  "        private int _field;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "AutoImplProperties", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.AutoImplPropertiesAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.AutoImplPropertiesAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new AutoImplPropertiesAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new AutoImplPropertiesAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new AutoImplPropertiesAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
