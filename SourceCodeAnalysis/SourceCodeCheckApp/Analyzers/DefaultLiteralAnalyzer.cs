using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class DefaultLiteralAnalyzer : SimpleAnalyzerBase
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.DefaultLiteralAnalyzer";

        public DefaultLiteralAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData> Detect(SyntaxNode node, SemanticModel model)
        {
            DefaultLiteralDetector detector = new DefaultLiteralDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} target-typed default literals";
        }

        protected override String CreateEntry(AnalyzerData entry)
        {
            return "Found target-typed default literal";
        }

        private class DefaultLiteralDetector : CSharpSyntaxWalker
        {
            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.IsKind(SyntaxKind.DefaultLiteralExpression))
                    Data.Add(new AnalyzerData(span));
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
