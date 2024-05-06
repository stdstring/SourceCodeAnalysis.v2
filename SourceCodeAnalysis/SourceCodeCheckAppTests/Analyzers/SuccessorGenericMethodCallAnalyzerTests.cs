using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests.Analyzers
{
    [TestFixture]
    public class SuccessorGenericMethodCallAnalyzerTests
    {
        [Test]
        public void ProcessWhenCallerGenericWithErrorLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<int>();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<T>();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            obj1.SomeOtherMethodG<bool>();\r\n" +
                                  "            obj1.SomeOtherMethodG<T>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<string>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<T>();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "            obj2.SomeAnotherMethodG<object>();\r\n" +
                                  "            obj2.SomeAnotherMethodG<T>();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<int>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(9): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(12): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<bool>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(13): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(15): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<string>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(16): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(19): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<object>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(20): [ERROR]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n";
            String expectedOutput = String.Format(expectedOutputTemplate, FilePath);
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Error);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCallerGenericWithWarningLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<int>();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<T>();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            obj1.SomeOtherMethodG<bool>();\r\n" +
                                  "            obj1.SomeOtherMethodG<T>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<string>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<T>();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "            obj2.SomeAnotherMethodG<object>();\r\n" +
                                  "            obj2.SomeAnotherMethodG<T>();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = "{0}(8): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<int>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(9): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(12): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<bool>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(13): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(15): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<string>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(16): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(19): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<object>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(20): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Warning);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCallerGenericWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<int>();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<T>();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            obj1.SomeOtherMethodG<bool>();\r\n" +
                                  "            obj1.SomeOtherMethodG<T>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<string>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<T>();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "            obj2.SomeAnotherMethodG<object>();\r\n" +
                                  "            obj2.SomeAnotherMethodG<T>();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            const String expectedOutputTemplate = $"Execution of {SuccessorGenericMethodCallAnalyzer.Name} started\r\n" +
                                                  "Found 8 calls of generic methods of successors from generic method of ancestor\r\n" +
                                                  "{0}(8): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<int>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(9): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(12): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<bool>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(13): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeIntermediateClass.SomeOtherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(15): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<string>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(16): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodSG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(19): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<object>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  "{0}(20): [{1}]: Found call of generic methods of successors \"SomeNamespace.SomeDerivedClass.SomeAnotherMethodG<T>()\" from generic method of ancestor \"SomeNamespace.SomeBaseClass.SomeMethod<T>()\"\r\n" +
                                                  $"Execution of {SuccessorGenericMethodCallAnalyzer.Name} finished\r\n";
            String expectedOnOutput = String.Format(expectedOutputTemplate, FilePath, "ERROR");
            String expectedWarningOutput = String.Format(expectedOutputTemplate, FilePath, "WARNING");
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, false, expectedOnOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, expectedWarningOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWhenCalledGenericVirtualMethod(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value)\r\n" +
                                  "        {\r\n" +
                                  "            SomeOtherMethod<int>(\"IDDQD\", 666);\r\n" +
                                  "            SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "            SomeBaseClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod<bool>(\"IDDQD\", true);\r\n" +
                                  "            obj1.SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "            SomeBaseClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeOtherMethod<string>(\"IDDQD\", \"IDKFA\");\r\n" +
                                  "            obj2.SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "        }\r\n" +
                                  "        public virtual void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public virtual void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public override void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public override void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public override void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public override void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCalledGenericVirtualMethodWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value)\r\n" +
                                  "        {\r\n" +
                                  "            SomeOtherMethod<int>(\"IDDQD\", 666);\r\n" +
                                  "            SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "            SomeBaseClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod<bool>(\"IDDQD\", true);\r\n" +
                                  "            obj1.SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "            SomeBaseClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeOtherMethod<string>(\"IDDQD\", \"IDKFA\");\r\n" +
                                  "            obj2.SomeOtherMethod<T>(\"IDDQD\", value);\r\n" +
                                  "        }\r\n" +
                                  "        public virtual void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public virtual void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public override void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public override void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public override void SomeOtherMethod<T>(string p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public override void SomeAnotherMethod<T>(int p1, T p2)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWhenCalledGenericSameDefined(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass.SomeAnotherMethod<int>();\r\n" +
                                  "            SomeClass.SomeAnotherMethod<T>();\r\n" +
                                  "            SomeClass obj1 = new SomeClass();\r\n" +
                                  "            obj1.SomeOtherMethod<bool>();\r\n" +
                                  "            obj1.SomeOtherMethod<T>();\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCalledGenericSameDefinedWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeClass.SomeAnotherMethod<int>();\r\n" +
                                  "            SomeClass.SomeAnotherMethod<T>();\r\n" +
                                  "            SomeClass obj1 = new SomeClass();\r\n" +
                                  "            obj1.SomeOtherMethod<bool>();\r\n" +
                                  "            obj1.SomeOtherMethod<T>();\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWhenCalledNonGeneric(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCalledNonGenericWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [TestCase(OutputLevel.Error)]
        [TestCase(OutputLevel.Warning)]
        public void ProcessWhenCallerNonGeneric(OutputLevel outputLevel)
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<int>();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            obj1.SomeOtherMethodG<int>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<string>();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "            obj2.SomeAnotherMethodG<string>();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, outputLevel);
            analyzerHelper.Process(_analyzerOnFactory, true, "");
            analyzerHelper.Process(_analyzerWarningFactory, true, "");
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void ProcessWhenCallerNonGenericWithInfoLevel()
        {
            const String source = "namespace SomeNamespace\r\n" +
                                  "{\r\n" +
                                  "    public class SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod()\r\n" +
                                  "        {\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodS();\r\n" +
                                  "            SomeIntermediateClass.SomeOtherMethodSG<int>();\r\n" +
                                  "            SomeIntermediateClass obj1 = new SomeIntermediateClass();\r\n" +
                                  "            obj1.SomeOtherMethod();\r\n" +
                                  "            obj1.SomeOtherMethodG<int>();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodS();\r\n" +
                                  "            SomeDerivedClass.SomeAnotherMethodSG<string>();\r\n" +
                                  "            SomeDerivedClass obj2 = new SomeDerivedClass();\r\n" +
                                  "            obj2.SomeAnotherMethod();\r\n" +
                                  "            obj2.SomeAnotherMethodG<string>();\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeIntermediateClass : SomeBaseClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeOtherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeOtherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeOtherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeDerivedClass : SomeIntermediateClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeAnotherMethod()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodS()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void SomeAnotherMethodG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public static void SomeAnotherMethodSG<T>()\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}\r\n";
            AnalyzerHelper analyzerHelper = new AnalyzerHelper(source, "SuccessorGenericMethodCall", FilePath, OutputLevel.Info);
            analyzerHelper.Process(_analyzerOnFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerWarningFactory, true, SourceCodeCheckAppOutputDef.SuccessorGenericMethodCallAnalyzerSuccessOutput);
            analyzerHelper.Process(_analyzerOffFactory, true, "");
        }

        [Test]
        public void CheckAnalyzerInfo()
        {
            AnalyzerInfo expectedInfo = new AnalyzerInfo(SuccessorGenericMethodCallAnalyzer.Name, SuccessorGenericMethodCallAnalyzer.Description);
            IOutput nullOutput = new NullOutput();
            Assert.That(_analyzerOnFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerWarningFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
            Assert.That(_analyzerOffFactory(nullOutput).AnalyzerInfo, Is.EqualTo(expectedInfo));
        }

        private readonly Func<IOutput, IFileAnalyzer> _analyzerOnFactory = output => new SuccessorGenericMethodCallAnalyzer(output, AnalyzerState.On);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerWarningFactory = output => new SuccessorGenericMethodCallAnalyzer(output, AnalyzerState.ErrorAsWarning);
        private readonly Func<IOutput, IFileAnalyzer> _analyzerOffFactory = output => new SuccessorGenericMethodCallAnalyzer(output, AnalyzerState.Off);

        private const String FilePath = "C:\\SomeFolder\\SomeClass.cs";
    }
}
