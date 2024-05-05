using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class CastToSameTypeAnalyzerTests
    {
        [Test]
        public void ProcessErrorCastsWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (System.String)\"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = "{0}(8): [ERROR]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}(9): [ERROR]: Found cast to the same type \"string\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessErrorCastsWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (System.String)\"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = "{0}(8): [{1}]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}(9): [{1}]: Found cast to the same type \"string\"\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessErrorCastsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            string s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string)s1;\r\n" +
                                  "            string s3 = (System.String)\"IDKFA\";\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            const String expectedOutputTemplate = $"Execution of {CastToSameTypeAnalyzer.Name} started\r\n" +
                                                  "Found 2 casts to the same type leading to errors\r\n" +
                                                  "{0}(8): [{1}]: Found cast to the same type \"string\"\r\n" +
                                                  "{0}(9): [{1}]: Found cast to the same type \"string\"\r\n" +
                                                  "Found 0 casts to the same type not leading to errors\r\n" +
                                                  $"Execution of {CastToSameTypeAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWarningCastsWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            double d1 = 3.14;\r\n" +
                                  "            double d2 = (double)d1;\r\n" +
                                  "            object obj1 = new object();\r\n" +
                                  "            object obj2 = (object)obj1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeDerivedClass someObj2 = (SomeDerivedClass)someObj1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWarningCastsWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            double d1 = 3.14;\r\n" +
                                  "            double d2 = (double)d1;\r\n" +
                                  "            object obj1 = new object();\r\n" +
                                  "            object obj2 = (object)obj1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeDerivedClass someObj2 = (SomeDerivedClass)someObj1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(14): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}(15): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}(17): [WARNING]: Found cast to the same type \"double\"\r\n" +
                                                  "{0}(19): [WARNING]: Found cast to the same type \"object\"\r\n" +
                                                  "{0}(21): [WARNING]: Found cast to the same type \"SomeNamespace.SomeDerivedClass\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWarningCastsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            int i2 = (int)i1;\r\n" +
                                  "            int i3 = (int)13;\r\n" +
                                  "            double d1 = 3.14;\r\n" +
                                  "            double d2 = (double)d1;\r\n" +
                                  "            object obj1 = new object();\r\n" +
                                  "            object obj2 = (object)obj1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeDerivedClass someObj2 = (SomeDerivedClass)someObj1;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {CastToSameTypeAnalyzer.Name} started\r\n" +
                                                  "Found 0 casts to the same type leading to errors\r\n" +
                                                  "Found 5 casts to the same type not leading to errors\r\n" +
                                                  "{0}(14): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}(15): [WARNING]: Found cast to the same type \"int\"\r\n" +
                                                  "{0}(17): [WARNING]: Found cast to the same type \"double\"\r\n" +
                                                  "{0}(19): [WARNING]: Found cast to the same type \"object\"\r\n" +
                                                  "{0}(21): [WARNING]: Found cast to the same type \"SomeNamespace.SomeDerivedClass\"\r\n" +
                                                  $"Execution of {CastToSameTypeAnalyzer.Name} finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessOtherCasts(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            long i2 = (long) i1;\r\n" +
                                  "            short i3 = (short) i1;\r\n" +
                                  "            uint i4 = (uint) i1;\r\n" +
                                  "            object s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string) s1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeBaseClass someObj2 = (SomeBaseClass) someObj1;\r\n" +
                                  "            SomeDerivedClass someObj3 = (SomeDerivedClass) someObj2;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessOtherCastsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i1 = 666;\r\n" +
                                  "            long i2 = (long) i1;\r\n" +
                                  "            short i3 = (short) i1;\r\n" +
                                  "            uint i4 = (uint) i1;\r\n" +
                                  "            object s1 = \"IDDQD\";\r\n" +
                                  "            string s2 = (string) s1;\r\n" +
                                  "            SomeDerivedClass someObj1 = new SomeDerivedClass();\r\n" +
                                  "            SomeBaseClass someObj2 = (SomeBaseClass) someObj1;\r\n" +
                                  "            SomeDerivedClass someObj3 = (SomeDerivedClass) someObj2;\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWithoutCasts(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i = 0;\r\n" +
                                  "            string s = \"IDDQD\";\r\n" +
                                  "            bool b = true;\r\n" +
                                  "            object obj = new object();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWithoutCastsWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            int i = 0;\r\n" +
                                  "            string s = \"IDDQD\";\r\n" +
                                  "            bool b = true;\r\n" +
                                  "            object obj = new object();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "CastToSameType", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(CastToSameTypeAnalyzer.Name, CastToSameTypeAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new CastToSameTypeAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new CastToSameTypeAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new CastToSameTypeAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
