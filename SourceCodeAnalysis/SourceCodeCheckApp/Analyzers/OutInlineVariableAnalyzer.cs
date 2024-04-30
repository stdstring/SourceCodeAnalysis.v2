using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class OutInlineVariableAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.OutInlineVariableAnalyzer";

        public OutInlineVariableAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            OutInlineVariableDetector detector = new OutInlineVariableDetector(model);
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} out inline variables");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData<String> entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, $"Found out inline variable when call: {entry.Data}");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

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
