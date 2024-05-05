using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NameOfExprAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NameOfExprAnalyzer";

        public NameOfExprAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            NameOfDetector detector = new NameOfDetector(model);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} nameof expressions";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found nameof expression: {entry.Data}";
        }

        private class NameOfDetector : CSharpSyntaxWalker
        {
            public NameOfDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                switch (node.Expression)
                {
                    case IdentifierNameSyntax {Identifier.Text: "nameof"}:
                        SymbolInfo symbolInfo = _model.GetSymbolInfo(node);
                        if (symbolInfo.Symbol is null)
                            Data.Add(new AnalyzerData<String>(node.ToString(), span));
                        break;
                }
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
        }
    }
}
