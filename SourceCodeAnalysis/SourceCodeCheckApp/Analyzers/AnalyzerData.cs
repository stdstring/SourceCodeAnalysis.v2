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

    internal abstract record TypeData(String FullName)
    {
        internal record UsualType(String NamespaceName, String TypeName) :
            TypeData($"{NamespaceName}.{TypeName}");

        internal record ArrayType(TypeData ElementType, Int32 Rank) :
            TypeData($"{ElementType.FullName}{String.Join("", Enumerable.Repeat("[]", Rank))}");

        public static TypeData Create(ITypeSymbol type)
        {
            return type switch
            {
                IArrayTypeSymbol{ElementType: var elementType, Rank: var rank} => new ArrayType(Create(elementType), rank),
                _ => new UsualType(type.ContainingNamespace.ToDisplayString(), type.Name)
            };
        }
    }
}