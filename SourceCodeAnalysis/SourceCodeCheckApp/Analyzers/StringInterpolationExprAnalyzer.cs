using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class StringInterpolationExprAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.StringInterpolationExprAnalyzer";

        public StringInterpolationExprAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            StringInterpolationExprDetector detector = new StringInterpolationExprDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} string interpolation expressions";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found string interpolation expression: {entry.Data}";
        }

        private class StringInterpolationExprDetector : CSharpSyntaxWalker
        {
            public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                Data.Add(new AnalyzerData<String>(node.ToString(), span));
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();
        }
    }
}
