using Microsoft.CodeAnalysis;

namespace SourceCodeCheckApp.Analyzers
{
    public record AnalyzerInfo(String Name, String Description);

    public interface IFileAnalyzer
    {
        // TODO (std_string) : think about parameters & return value
        Boolean Process(String filePath, SyntaxTree tree, SemanticModel model);
        AnalyzerInfo AnalyzerInfo { get; }
    }
}