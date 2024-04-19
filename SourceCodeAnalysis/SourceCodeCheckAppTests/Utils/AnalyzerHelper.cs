using Microsoft.CodeAnalysis;
using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckAppTests.Utils
{
    internal static class AnalyzerHelper
    {
        public static void Process(Func<IOutput, IFileAnalyzer> analyzerFactory, String source, String assemblyName, String filePath, OutputLevel outputLevel, Boolean expectedResult, String expectedOutput)
        {
            SemanticModel model = PreparationHelper.Prepare(source, assemblyName);
            using (TextWriter outputWriter = new StringWriter())
            using (TextWriter errorWriter = new StringWriter())
            {
                OutputImpl output = new OutputImpl(outputWriter, errorWriter, outputLevel);
                IFileAnalyzer analyzer = analyzerFactory(output);
                Boolean actualResult = analyzer.Process(filePath, model.SyntaxTree, model);
                Assert.That(actualResult, Is.EqualTo(expectedResult));
                String actualOutput = outputWriter.ToString() ?? "";
                Assert.That(actualOutput, Is.EqualTo(expectedOutput));
                String actualError = errorWriter.ToString() ?? "";
                Assert.That(actualError, Is.Empty);
            }
        }
    }
}