using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceCodeCheckApp.Analyzers
{
    internal record AnalyzerData<T>(T Data, LinePosition StartPosition, LinePosition EndPosition)
    {
        public AnalyzerData(T data, FileLinePositionSpan span) : this(data, span.StartLinePosition, span.EndLinePosition)
        {
        }
    }

    internal record TypeData(String NamespaceName, String TypeName)
    {
        public TypeData(ITypeSymbol type) : this(type.ContainingNamespace.ToDisplayString(), type.Name)
        {
        }

        public readonly String FullName = $"{NamespaceName}.{TypeName}";
    }
}