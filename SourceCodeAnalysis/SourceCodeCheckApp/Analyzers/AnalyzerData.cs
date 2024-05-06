using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceCodeCheckApp.Analyzers
{
    internal record AnalyzerData(LinePosition StartPosition, LinePosition EndPosition)
    {
        public AnalyzerData(FileLinePositionSpan span) : this(span.StartLinePosition, span.EndLinePosition)
        {
        }
    }

    internal record AnalyzerData<T>(T Data, LinePosition StartPosition, LinePosition EndPosition)
    {
        public AnalyzerData(T data, FileLinePositionSpan span) : this(data, span.StartLinePosition, span.EndLinePosition)
        {
        }
    }

    internal record SimpleTypeData(String TypeName, String FullName);
}