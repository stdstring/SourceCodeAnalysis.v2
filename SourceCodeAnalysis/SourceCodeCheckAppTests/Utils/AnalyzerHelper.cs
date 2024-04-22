using Microsoft.CodeAnalysis;
using NUnit.Framework;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckAppTests.Utils
{
    internal class AnalyzerHelper
    {
        public AnalyzerHelper(String source, String assemblyName, String filePath, OutputLevel outputLevel)
        {
            _source = source;
            _assemblyName = assemblyName;
            _filePath = filePath;
            _outputLevel = outputLevel;
        }

        public void Process(Func<IOutput, IFileAnalyzer> analyzerFactory, Boolean expectedResult, String expectedOutput)
        {
            SemanticModel model = PreparationHelper.Prepare(_source, _assemblyName);
            using (TextWriter outputWriter = new StringWriter())
            using (TextWriter errorWriter = new StringWriter())
            {
                OutputImpl output = new OutputImpl(outputWriter, errorWriter, _outputLevel);
                IFileAnalyzer analyzer = analyzerFactory(output);
                Boolean actualResult = analyzer.Process(_filePath, model.SyntaxTree, model);
                String actualOutput = outputWriter.ToString() ?? "";
                String actualError = errorWriter.ToString() ?? "";
                Assert.Multiple(() =>
                {
                    Assert.That(actualResult, Is.EqualTo(expectedResult));
                    Assert.That(actualOutput, Is.EqualTo(expectedOutput));
                    Assert.That(actualError, Is.Empty);
                });
            }
        }

        private readonly String _source;
        private readonly String _assemblyName;
        private readonly String _filePath;
        private readonly OutputLevel _outputLevel;
    }
}