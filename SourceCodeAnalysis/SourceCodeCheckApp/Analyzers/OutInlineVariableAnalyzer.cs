using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class OutInlineVariableAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.OutInlineVariableAnalyzer";

        public const String Description = "This analyzer finds out inline variables. All such variables are considered as errors.";

        public OutInlineVariableAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name, Description)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            OutInlineVariableDetector detector = new OutInlineVariableDetector(model);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} out inline variables";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found out inline variable when call: {entry.Data}";
        }

        private class OutInlineVariableDetector : CSharpSyntaxWalker
        {
            public OutInlineVariableDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                foreach (ArgumentSyntax argument in node.ArgumentList.Arguments)
                {
                    if (!argument.RefKindKeyword.IsKind(SyntaxKind.OutKeyword))
                        continue;
                    if (argument.Expression is not DeclarationExpressionSyntax)
                        continue;
                    SymbolInfo symbolInfo = _model.GetSymbolInfo(node);
                    if (symbolInfo.Symbol == null)
                        throw new InvalidOperationException($"Bad symbol info for invocation expression: {node}");
                    Data.Add(new AnalyzerData<String>(symbolInfo.Symbol.ToDisplayString(), span));
                }
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
        }
    }
}
