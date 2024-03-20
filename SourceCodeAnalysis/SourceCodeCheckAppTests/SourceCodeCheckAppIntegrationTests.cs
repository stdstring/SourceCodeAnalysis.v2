﻿using NUnit.Framework;
using SourceCodeCheckAppTests.Utils;

namespace SourceCodeCheckAppTests
{
    [TestFixture]
    public class SourceCodeCheckAppIntegrationTests
    {
        [Test]
        public void ProcessEmptyArgs()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("");
            ExecutionChecker.Check(executionResult, 0, SourceCodeCheckAppOutputDef.AppDescription, "");
        }

        [Test]
        public void ProcessHelp()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--help");
            ExecutionChecker.Check(executionResult, 0, SourceCodeCheckAppOutputDef.AppDescription, "");
        }

        [Test]
        public void ProcessVersion()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--version");
            ExecutionChecker.Check(executionResult, 0, "0.0.1", "");
        }

        [TestCase("--some-strange-option")]
        [TestCase("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\GoodExampleErrorDefault.xml\" --some-strange-option")]
        public void ProcessUnknownArg(String args)
        {
            ExecutionResult executionResult = ExecutionHelper.Execute(args);
            ExecutionChecker.Check(executionResult, -1, SourceCodeCheckAppOutputDef.AppDescription, SourceCodeCheckAppOutputDef.BadArgsMessage);
        }

        [Test]
        public void ProcessConfigWithoutValue()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=");
            ExecutionChecker.Check(executionResult, -1, "", SourceCodeCheckAppOutputDef.BadConfigMessage);
        }

        [Test]
        public void ProcessUnknownSource()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\UnknownSourceErrorDefault.xml\"");
            ExecutionChecker.Check(executionResult, -1, "", SourceCodeCheckAppOutputDef.UnknownSourceMessage);
        }

        [Test]
        public void ProcessGoodExampleProjectError()
        {
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\GoodExampleErrorDefault.xml\"");
            ExecutionChecker.Check(executionResult, 0, "", "");
        }

        [Test]
        public void ProcessGoodExampleProjectWarning()
        {
            const String projectFilename = "..\\..\\..\\..\\Examples\\TestSolution\\GoodExample\\GoodExample.csproj";
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\GoodExampleWarningDefault.xml\"");
            String projectDir = Path.GetFullPath(Path.GetDirectoryName(projectFilename)!);
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(20): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"SomeBaseLibrary.SomeBaseClass\"\r\n" +
                                                  "{0}\\ClassNameExample.cs(7): [WARNING]: Found type named \"GoodExample.Classnameexample\" which corresponds the filename \"ClassNameExample.cs\" only at ignoring case";
            String expectedOutput = String.Format(expectedOutputTemplate, projectDir);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }

        [Test]
        public void ProcessGoodExampleProjectInfo()
        {
            String projectFilename = Path.GetFullPath("..\\..\\..\\..\\Examples\\TestSolution\\GoodExample\\GoodExample.csproj");
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\GoodExampleInfoDefault.xml\"");
            String projectDir = Path.GetFullPath(Path.GetDirectoryName(projectFilename)!);
            const String expectedOutputTemplate = "Processing of the project {0} is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.NugetRestoreOutput +
                                                  SourceCodeCheckAppOutputDef.CompilationCheckSuccessOutput +
                                                  //"Processing of the file {1}\\CastsExample.cs is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 0 casts leading to errors in the ported C++ code\r\n" +
                                                  "Found 2 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(20): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{1}\\CastsExample.cs(24): [WARNING]: Found cast to the same type \"SomeBaseLibrary.SomeBaseClass\"\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n" +
                                                  SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  //"Processing of the file {1}\\CastsExample.cs is finished\r\n" +
                                                  //"Processing of the file {1}\\ClassNameExample.cs is started\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "File contains 1 types with names match to the filename with ignoring case\r\n" +
                                                  "{1}\\ClassNameExample.cs(7): [WARNING]: Found type named \"GoodExample.Classnameexample\" which corresponds the filename \"ClassNameExample.cs\" only at ignoring case\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n" +
                                                  SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  //"Processing of the file {1}\\ClassNameExample.cs is finished\r\n" +
                                                  //"Processing of the file {1}\\IdentifiersExample.cs is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  //"Processing of the file {1}\\IdentifiersExample.cs is finished\r\n" +
                                                  "Processing of the project {0} is finished\r\n" +
                                                  "Result of analysis: analysis is succeeded";
            String expectedOutput = String.Format(expectedOutputTemplate, projectFilename, projectDir);
            ExecutionChecker.Check(executionResult, 0, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectError()
        {
            String projectFilename = Path.GetFullPath("..\\..\\..\\..\\Examples\\TestSolution\\BadExample\\BadExample.csproj");
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\BadExampleErrorDefault.xml\"");
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(22): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "{0}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\IdentifiersExample.cs(5): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(13): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(17): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(21): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(28): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам2\"";
            String projectDir = Path.GetDirectoryName(projectFilename)!;
            String expectedOutput = String.Format(expectedOutputTemplate, projectDir);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectWarning()
        {
            String projectFilename = Path.GetFullPath("..\\..\\..\\..\\Examples\\TestSolution\\BadExample\\BadExample.csproj");
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\BadExampleWarningDefault.xml\"");
            const String expectedOutputTemplate = "{0}\\CastsExample.cs(22): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "{0}\\CastsExample.cs(20): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{0}\\CastsExample.cs(25): [WARNING]: Found cast to the same type \"SomeBaseLibrary.SomeBaseClass\"\r\n" +
                                                  "{0}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{0}\\IdentifiersExample.cs(5): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(13): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(17): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(21): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(28): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{0}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам2\"";
            String projectDir = Path.GetDirectoryName(projectFilename)!;
            String expectedOutput = String.Format(expectedOutputTemplate, projectDir);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }

        [Test]
        public void ProcessBadExampleProjectInfo()
        {
            String projectFilename = Path.GetFullPath("..\\..\\..\\..\\Examples\\TestSolution\\BadExample\\BadExample.csproj");
            ExecutionResult executionResult = ExecutionHelper.Execute("--config=\"..\\..\\..\\..\\Examples\\TestConfigs\\BadExampleInfoDefault.xml\"");
            const String expectedOutputTemplate = "Processing of the project {0} is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.NugetRestoreOutput +
                                                  SourceCodeCheckAppOutputDef.CompilationCheckSuccessOutput +
                                                  //"Processing of the file {1}\\CastsExample.cs is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  "Execution of CastToSameTypeAnalyzer started\r\n" +
                                                  "Found 1 casts leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(22): [ERROR]: Found cast to the same type \"System.String\"\r\n" +
                                                  "Found 2 casts to the same type not leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\CastsExample.cs(20): [WARNING]: Found cast to the same type \"System.Int32\"\r\n" +
                                                  "{1}\\CastsExample.cs(25): [WARNING]: Found cast to the same type \"SomeBaseLibrary.SomeBaseClass\"\r\n" +
                                                  "Execution of CastToSameTypeAnalyzer finished\r\n" +
                                                  SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  //"Processing of the file {1}\\CastsExample.cs is finished\r\n" +
                                                  //"Processing of the file {1}\\ClassnameExample.cs is started\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer started\r\n" +
                                                  "File doesn't contain any type with name exact match to the filename, but contains 2 types with names match to the filename with ignoring case\r\n" +
                                                  "{1}\\ClassnameExample.cs(3): [ERROR]: Found type named \"BadExample.ClassNameExample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "{1}\\ClassnameExample.cs(7): [ERROR]: Found type named \"BadExample.Classnameexample\" which corresponds the filename \"ClassnameExample.cs\" only at ignoring case\r\n" +
                                                  "Execution of BadFilenameCaseAnalyzer finished\r\n" +
                                                  SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  SourceCodeCheckAppOutputDef.NonAsciiIdentifiersAnalyzerSuccessOutput +
                                                  //"Processing of the file {1}\\ClassnameExample.cs is finished\r\n" +
                                                  //"Processing of the file {1}\\IdentifiersExample.cs is started\r\n" +
                                                  SourceCodeCheckAppOutputDef.BadFilenameCaseAnalyzerSuccessOutput +
                                                  SourceCodeCheckAppOutputDef.CastToSameTypeAnalyzerSuccessOutput +
                                                  "Execution of NonAsciiIdentifiersAnalyzer started\r\n" +
                                                  "Found 14 non-ASCII identifiers leading to errors in the ported C++ code\r\n" +
                                                  "{1}\\IdentifiersExample.cs(5): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(13): [ERROR]: Found non-ASCII identifier \"TPаrаm1\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(17): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(21): [ERROR]: Found non-ASCII identifier \"ISomеInterface\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(27): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(28): [ERROR]: Found non-ASCII identifier \"somеObjB\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(29): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(30): [ERROR]: Found non-ASCII identifier \"someImplОbj\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"SоmeSimpleClassA\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"локальноеДействие\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам1\"\r\n" +
                                                  "{1}\\IdentifiersExample.cs(31): [ERROR]: Found non-ASCII identifier \"парам2\"\r\n" +
                                                  "Execution of NonAsciiIdentifiersAnalyzer finished\r\n" +
                                                  //"Processing of the file {1}\\IdentifiersExample.cs is finished\r\n" +
                                                  "Processing of the project {0} is finished\r\n" +
                                                  "Result of analysis: analysis is failed";
            String projectDir = Path.GetDirectoryName(projectFilename)!;
            String expectedOutput = String.Format(expectedOutputTemplate, projectFilename, projectDir);
            ExecutionChecker.Check(executionResult, -1, expectedOutput, "");
        }
    }
}
