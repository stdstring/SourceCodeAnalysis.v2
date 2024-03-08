using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;

namespace SourceCodeCheckApp.Processors
{
    internal class FileProcessorHelper
    {
        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers, ConfigData externalData)
        {
            Boolean result = true;
            foreach (IFileAnalyzer analyzer in analyzers)
            {
                result &= analyzer.Process(filename, tree, model, externalData);
            }
            return result;
        }

    }
}