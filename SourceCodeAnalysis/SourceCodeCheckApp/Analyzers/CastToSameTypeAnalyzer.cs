using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class CastToSameTypeAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer";

        public CastToSameTypeAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(filePath, detector.Data);
            ProcessWarnings(filePath, detector.Data);
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || !hasErrors;
        }

        private Boolean ProcessErrors(String filePath, IList<AnalyzerData<String>> data)
        {
            IList<AnalyzerData<String>> errors = data.Where(item => _errorCastTypes.Contains(item.Data)).ToList();
            _output.WriteInfoLine($"Found {errors.Count} casts leading to errors in the ported C++ code");
            foreach (AnalyzerData<String> error in errors)
                _output.WriteErrorLine(filePath, error.StartPosition.Line, $"Found cast to the same type \"{error.Data}\"");
            return errors.Count > 0;
        }

        private void ProcessWarnings(String filePath, IList<AnalyzerData<String>> data)
        {
            IList<AnalyzerData<String>> warnings = data.Where(item => !_errorCastTypes.Contains(item.Data)).ToList();
            _output.WriteInfoLine($"Found {warnings.Count} casts to the same type not leading to errors in the ported C++ code");
            foreach (AnalyzerData<String> warning in warnings)
                _output.WriteWarningLine(filePath, warning.StartPosition.Line, $"Found cast to the same type \"{warning.Data}\"");
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private readonly String[] _errorCastTypes = new[]{"string", "System.String"};

        private class CastToSameTypeDetector : CSharpSyntaxWalker
        {
            public CastToSameTypeDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitCastExpression(CastExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                TypeSyntax typeSyntax = node.Type;
                ExpressionSyntax expressionSyntax = node.Expression;
                ITypeSymbol? type = _model.GetTypeInfo(typeSyntax).Type;
                switch (expressionSyntax)
                {
                    case LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.NullLiteralExpression):
                        break;
                    default:
                    {
                        ITypeSymbol? expressionType = _model.GetTypeInfo(expressionSyntax).Type;
                        if ((type == null) || (expressionType == null))
                            throw new InvalidOperationException();
                        if (type.Equals(expressionType, SymbolEqualityComparer.Default))
                            Data.Add(new AnalyzerData<String>(type.ToDisplayString(), span));
                        break;
                    }
                }
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
        }
    }
}
