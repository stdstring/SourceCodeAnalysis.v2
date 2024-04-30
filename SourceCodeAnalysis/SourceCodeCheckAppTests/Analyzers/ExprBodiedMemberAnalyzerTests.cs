using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class ExprBodiedMemberAnalyzerTests
    {
        [Test]
        public void ProcessExprBodiedMembersWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int value) => _value = value;\r\n" +
                                  "        public int Prop1 => _value;\r\n" +
                                  "        public int Prop2 { get => _value; set => _value = value; }\r\n" +
                                  "        public int Prop3 { get => _value; set { _value = value; } }\r\n" +
                                  "        public int this[int index] { get => _value; set => _value = value; }\r\n" +
                                  "        public void SetValue(int value) => _value = value;\r\n" +
                                  "        public int GetValue() => _value;\r\n" +
                                  "        private int _value;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [ERROR]: Found expression-bodied ctor: SomeNamespace.SomeClass.SomeClass(int)\r\n" +
                                                  "{0}(6): [ERROR]: Found expression-bodied property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(7): [ERROR]: Found expression-bodied property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(8): [ERROR]: Found expression-bodied property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(9): [ERROR]: Found expression-bodied indexer: SomeNamespace.SomeClass.this[int]\r\n" +
                                                  "{0}(10): [ERROR]: Found expression-bodied method: SomeNamespace.SomeClass.SetValue(int)\r\n" +
                                                  "{0}(11): [ERROR]: Found expression-bodied method: SomeNamespace.SomeClass.GetValue()\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExprBodiedMembers", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessExprBodiedMembersWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int value) => _value = value;\r\n" +
                                  "        public int Prop1 => _value;\r\n" +
                                  "        public int Prop2 { get => _value; set => _value = value; }\r\n" +
                                  "        public int Prop3 { get => _value; set { _value = value; } }\r\n" +
                                  "        public int this[int index] { get => _value; set => _value = value; }\r\n" +
                                  "        public void SetValue(int value) => _value = value;\r\n" +
                                  "        public int GetValue() => _value;\r\n" +
                                  "        private int _value;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(5): [{1}]: Found expression-bodied ctor: SomeNamespace.SomeClass.SomeClass(int)\r\n" +
                                                  "{0}(6): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(7): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(8): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(9): [{1}]: Found expression-bodied indexer: SomeNamespace.SomeClass.this[int]\r\n" +
                                                  "{0}(10): [{1}]: Found expression-bodied method: SomeNamespace.SomeClass.SetValue(int)\r\n" +
                                                  "{0}(11): [{1}]: Found expression-bodied method: SomeNamespace.SomeClass.GetValue()\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExprBodiedMembers", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessExprBodiedMembersWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int value) => _value = value;\r\n" +
                                  "        public int Prop1 => _value;\r\n" +
                                  "        public int Prop2 { get => _value; set => _value = value; }\r\n" +
                                  "        public int Prop3 { get => _value; set { _value = value; } }\r\n" +
                                  "        public int this[int index] { get => _value; set => _value = value; }\r\n" +
                                  "        public void SetValue(int value) => _value = value;\r\n" +
                                  "        public int GetValue() => _value;\r\n" +
                                  "        private int _value;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {ExprBodiedMemberAnalyzer.Name} started\r\n" +
                                                  "Found 7 expression-bodied members\r\n" +
                                                  "{0}(5): [{1}]: Found expression-bodied ctor: SomeNamespace.SomeClass.SomeClass(int)\r\n" +
                                                  "{0}(6): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop1\r\n" +
                                                  "{0}(7): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop2\r\n" +
                                                  "{0}(8): [{1}]: Found expression-bodied property: SomeNamespace.SomeClass.Prop3\r\n" +
                                                  "{0}(9): [{1}]: Found expression-bodied indexer: SomeNamespace.SomeClass.this[int]\r\n" +
                                                  "{0}(10): [{1}]: Found expression-bodied method: SomeNamespace.SomeClass.SetValue(int)\r\n" +
                                                  "{0}(11): [{1}]: Found expression-bodied method: SomeNamespace.SomeClass.GetValue()\r\n" +
                                                  $"Execution of {ExprBodiedMemberAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExprBodiedMembers", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutExprBodiedMembers(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int value)\r\n" +
                                  "        {\r\n" +
                                  "            _value = value;\r\n" +
                                  "        }\r\n" +
                                  "        public int Prop1 { get { return _value; } }\r\n" +
                                  "        public int Prop2 { get { return _value; } set { _value = value; } }\r\n" +
                                  "        public int Prop3 { get; set; }\r\n" +
                                  "        public int this[int index] { get { return _value; } set { _value = value; } }\r\n" +
                                  "        public void SetValue(int value)\r\n" +
                                  "        {\r\n" +
                                  "            _value = value;\r\n" +
                                  "        }\r\n" +
                                  "        public int GetValue()\r\n" +
                                  "        {\r\n" +
                                  "            return _value;\r\n" +
                                  "        }\r\n" +
                                  "        private int _value;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExprBodiedMembers", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutExprBodiedMembersWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public SomeClass(int value)\r\n" +
                                  "        {\r\n" +
                                  "            _value = value;\r\n" +
                                  "        }\r\n" +
                                  "        public int Prop1 { get { return _value; } }\r\n" +
                                  "        public int Prop2 { get { return _value; } set { _value = value; } }\r\n" +
                                  "        public int Prop3 { get; set; }\r\n" +
                                  "        public int this[int index] { get { return _value; } set { _value = value; } }\r\n" +
                                  "        public void SetValue(int value)\r\n" +
                                  "        {\r\n" +
                                  "            _value = value;\r\n" +
                                  "        }\r\n" +
                                  "        public int GetValue()\r\n" +
                                  "        {\r\n" +
                                  "            return _value;\r\n" +
                                  "        }\r\n" +
                                  "        private int _value;\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "ExprBodiedMembers", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.ExprBodiedMemberAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.ExprBodiedMemberAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new ExprBodiedMemberAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new ExprBodiedMemberAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new ExprBodiedMemberAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
