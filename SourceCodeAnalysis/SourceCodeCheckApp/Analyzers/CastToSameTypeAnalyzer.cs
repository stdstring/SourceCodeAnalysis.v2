using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class CastToSameTypeAnalyzer : IFileAnalyzer
    {
        public CastToSameTypeAnalyzer(OutputImpl output)
        {
            _output = output;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteInfoLine($"Execution of CastToSameTypeAnalyzer started");
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(filePath, detector.Data);
            ProcessWarnings(filePath, detector.Data);
            _output.WriteInfoLine($"Execution of CastToSameTypeAnalyzer finished");
            return !hasErrors;
        }

        private Boolean ProcessErrors(String filePath, IList<AnalyzerData<TypeData>> data)
        {
            IList<AnalyzerData<TypeData>> errors = data.Where(item => _errorCastTypes.Contains(item.Data.FullName)).ToList();
            _output.WriteInfoLine($"Found {errors.Count} casts leading to errors in the ported C++ code");
            foreach (AnalyzerData<TypeData> error in errors)
                _output.WriteErrorLine(filePath, error.StartPosition.Line, $"Found cast to the same type \"{error.Data.FullName}\"");
            return errors.Count > 0;
        }

        private void ProcessWarnings(String filePath, IList<AnalyzerData<TypeData>> data)
        {
            IList<AnalyzerData<TypeData>> warnings = data.Where(item => !_errorCastTypes.Contains(item.Data.FullName)).ToList();
            _output.WriteInfoLine($"Found {warnings.Count} casts to the same type not leading to errors in the ported C++ code");
            foreach (AnalyzerData<TypeData> warning in warnings)
                _output.WriteWarningLine(filePath, warning.StartPosition.Line, $"Found cast to the same type \"{warning.Data.FullName}\"");
        }

        private readonly OutputImpl _output;

        private readonly String[] _errorCastTypes = {"System.String"};

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
                            Data.Add(new AnalyzerData<TypeData>(TypeData.Create(type), span));
                        break;
                    }
                }
            }

            public IList<AnalyzerData<TypeData>> Data { get; } = new List<AnalyzerData<TypeData>>();

            private readonly SemanticModel _model;
        }
    }
}
