using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NullCoalescingOperatorAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NullCoalescingOperatorAnalyzer";

        public NullCoalescingOperatorAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            NullCoalescingOperatorDetector detector = new NullCoalescingOperatorDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} null-coalescing operators";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found null-coalescing operator: \"{entry.Data}\"";
        }

        private class NullCoalescingOperatorDetector : CSharpSyntaxWalker
        {
            public override void VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (!node.IsKind(SyntaxKind.CoalesceExpression))
                    return;
                Data.Add(new AnalyzerData<String>("??", span));
            }

            public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (!node.IsKind(SyntaxKind.CoalesceAssignmentExpression))
                    return;
                Data.Add(new AnalyzerData<String>("??=", span));
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();
        }
    }
}
