using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal record CallData(String Caller, String Called);

    internal class SuccessorGenericMethodCallAnalyzer : SimpleAnalyzerBase<CallData>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.SuccessorGenericMethodCallAnalyzer";

        public SuccessorGenericMethodCallAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<CallData>> Detect(SyntaxNode node, SemanticModel model)
        {
            GenericMethodDetector detector = new GenericMethodDetector(model);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} calls of generic methods of successors from generic method of ancestor";
        }

        protected override String CreateEntry(AnalyzerData<CallData> entry)
        {
            return $"Found call of generic methods of successors \"{entry.Data.Called}\" from generic method of ancestor \"{entry.Data.Caller}\"";
        }

        private class GenericMethodDetector : CSharpSyntaxWalker
        {
            public GenericMethodDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                switch (_model.GetDeclaredSymbol(node))
                {
                    case null:
                        throw new InvalidOperationException($"Bad declaration info for {node.Identifier}");
                    case IMethodSymbol {IsGenericMethod: true, ContainingType: var containingType} methodSymbol:
                        String caller = methodSymbol.ToDisplayString();
                        SuccessorGenericMethodCallDetector detector = new SuccessorGenericMethodCallDetector(_model, containingType, caller);
                        detector.Visit(node.Body != null ? node.Body : node.ExpressionBody);
                        foreach (AnalyzerData<String> entry in detector.Data)
                        {
                            CallData callData = new CallData(caller, entry.Data);
                            Data.Add(new AnalyzerData<CallData>(callData, entry.StartPosition, entry.EndPosition));
                        }
                        break;
                }
            }

            public IList<AnalyzerData<CallData>> Data { get; } = new List<AnalyzerData<CallData>>();

            private readonly SemanticModel _model;
        }

        private class SuccessorGenericMethodCallDetector : CSharpSyntaxWalker
        {
            public SuccessorGenericMethodCallDetector(SemanticModel model, ITypeSymbol currentType, String caller)
            {
                _model = model;
                _currentType = currentType;
                _caller = caller;
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                SymbolInfo symbol = _model.GetSymbolInfo(node);
                switch (symbol.Symbol)
                {
                    case null:
                        throw new InvalidOperationException($"Bad invocation symbol info for {_caller}");
                    case IMethodSymbol {IsGenericMethod: true, ContainingType: var containingType} methodSymbol:
                        if (_currentType.Equals(containingType, SymbolEqualityComparer.Default))
                            break;
                        while ((containingType != null) && !_currentType.Equals(containingType, SymbolEqualityComparer.Default))
                            containingType = containingType.BaseType;
                        if (containingType != null)
                        {
                            FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                            Data.Add(new AnalyzerData<String>(methodSymbol.ToDisplayString(), span));
                        }
                        break;
                }
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
            private readonly ITypeSymbol _currentType;
            private readonly String _caller;
        }
    }
}
