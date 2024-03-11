using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Analyzers;

namespace SourceCodeCheckApp.Processors
{
    internal class FileProcessorHelper
    {
        public Boolean Process(String filename, SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            Boolean result = true;
            foreach (IFileAnalyzer analyzer in analyzers)
                result &= analyzer.Process(filename, tree, model);
            return result;
        }

    }
}