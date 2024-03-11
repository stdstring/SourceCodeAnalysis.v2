using Microsoft.CodeAnalysis;

namespace SourceCodeCheckApp.Analyzers
{
    public interface IFileAnalyzer
    {
        // TODO (std_string) : think about parameters & return value
        Boolean Process(String filePath, SyntaxTree tree, SemanticModel model);
    }
}