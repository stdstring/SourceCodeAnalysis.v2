using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NullConditionalOperatorAnalyzer : SimpleAnalyzerBase
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NullConditionalOperatorAnalyzer";

        public const String Description = "This analyzer finds all null-conditional operators. All such operators are considered as errors.";

        public NullConditionalOperatorAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name, Description)
        {
        }

        protected override IList<AnalyzerData> Detect(SyntaxNode node, SemanticModel model)
        {
            NullConditionalOperatorDetector detector = new NullConditionalOperatorDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} null-conditional operators";
        }

        protected override String CreateEntry(AnalyzerData entry)
        {
            return "Found null-conditional operator";
        }

        private class NullConditionalOperatorDetector : CSharpSyntaxWalker
        {
            public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                Data.Add(new AnalyzerData(span));
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
