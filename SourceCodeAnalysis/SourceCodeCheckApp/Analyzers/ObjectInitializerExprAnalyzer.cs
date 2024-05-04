using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class ObjectInitializerExprAnalyzer : SimpleAnalyzerBase
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ObjectInitializerExprAnalyzer";

        public ObjectInitializerExprAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData> Detect(SyntaxNode node, SemanticModel model)
        {
            ObjectInitializerExprDetector detector = new ObjectInitializerExprDetector();
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} object initializer expressions";
        }

        protected override String CreateEntry(AnalyzerData entry)
        {
            return "Found object initializer expressions";
        }

        private class ObjectInitializerExprDetector : CSharpSyntaxWalker
        {
            public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.Initializer is {Expressions.Count: > 0})
                    Data.Add(new AnalyzerData(span));
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
