using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class ChainedAssignmentAnalyzer : SimpleAnalyzerBase
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ChainedAssignmentAnalyzer";

        public ChainedAssignmentAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData> Detect(SyntaxNode node, SemanticModel model)
        {
            ChainedAssignmentDetector detector = new ChainedAssignmentDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} chained assignments";
        }

        protected override String CreateEntry(AnalyzerData entry)
        {
            return "Found chained assignments";
        }

        private class ChainedAssignmentDetector : CSharpSyntaxWalker
        {
            public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
            {
                switch (node)
                {
                    case {Parent: AssignmentExpressionSyntax}:
                        return;
                    case {Parent: EqualsValueClauseSyntax}:
                    {
                        FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Parent.Span);
                        Data.Add(new AnalyzerData(span));
                        return;
                    }
                    case {Right: AssignmentExpressionSyntax}:
                    {
                        FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                        Data.Add(new AnalyzerData(span));
                        return;
                    }
                }
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
