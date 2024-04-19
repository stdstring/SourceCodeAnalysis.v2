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
        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessErrorCasts(OutputLevel outputLevel)
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(8): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "{0}(9): [ERROR]: Found cast to the same type \"System.String\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, outputLevel, false, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 2 casts leading to errors in the ported C++ code\r\n" +
                                                  "{0}(8): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "{0}(9): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "Found 0 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Info, false, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Error, true, "");
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "{0}(14): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}(15): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}(17): [WARNING]: Found cast to the same type \"System.Double\"\r\n" +
                                                  "{0}(19): [WARNING]: Found cast to the same type \"System.Object\"\r\n" +
                                                  "{0}(21): [WARNING]: Found cast to the same type \"SomeNamespace.SomeDerivedClass\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Warning, true, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            const String expectedOutputTemplate = "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                                  "Found 5 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "{0}(14): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}(15): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}(17): [WARNING]: Found cast to the same type \"System.Double\"\r\n" +
                                                  "{0}(19): [WARNING]: Found cast to the same type \"System.Object\"\r\n" +
                                                  "{0}(21): [WARNING]: Found cast to the same type \"SomeNamespace.SomeDerivedClass\"\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, filePath);
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Info, true, expectedOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, outputLevel, true, "");
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Info, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, outputLevel, true, "");
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
            const String filePath = "C:\\SomeFolder\\SomeClass.cs";
            AnalyzerHelper.Process(_analyzerFactory, source, "CastToSameType", filePath, OutputLevel.Info, true, SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput);
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerFactory = output => new CastToSameTypeAnalyzer(output, AnalyzerState.On);
    }
}
