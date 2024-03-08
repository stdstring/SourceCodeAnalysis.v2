using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class CastToSameTypeAnalyzer : IFileAnalyzer
    {
        public CastToSameTypeAnalyzer(OutputImpl output)
        {
            _output = output;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model, ConfigData externalData)
        {
            _output.WriteInfoLine($"Execution of CastToSameTypeAnalyzer started");
            CastToSameTypeDetector detector = new CastToSameTypeDetector(model);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(filePath, detector.Data);
            ProcessWarnings(filePath, detector.Data);
            _output.WriteInfoLine($"Execution of CastToSameTypeAnalyzer finished");
            return !hasErrors;
        }

        private Boolean ProcessErrors(String filePath, IList<CollectedData<String>> data)
        {
            IList<CollectedData<String>> errors = data.Where(item => _errorCastTypes.Contains(item.Data)).ToList();
            _output.WriteInfoLine($"Found {errors.Count} casts leading to errors in the ported C++ code");
            foreach (CollectedData<String> error in errors)
                _output.WriteErrorLine(filePath, error.StartPosition.Line, $"Found cast to the same type \"{error.Data}\"");
            return errors.Count > 0;
        }

        private void ProcessWarnings(String filePath, IList<CollectedData<String>> data)
        {
            IList<CollectedData<String>> warnings = data.Where(item => !_errorCastTypes.Contains(item.Data)).ToList();
            _output.WriteInfoLine($"Found {warnings.Count} casts to the same type not leading to errors in the ported C++ code");
            foreach (CollectedData<String> warning in warnings)
                _output.WriteWarningLine(filePath, warning.StartPosition.Line, $"Found cast to the same type \"{warning.Data}\"");
        }

        private readonly OutputImpl _output;

        private readonly String[] _errorCastTypes = {"string", "System.String"};

        private class CastToSameTypeDetector : CSharpSyntaxWalker
        {
            public CastToSameTypeDetector(SemanticModel model)
            {
                _model = model;
                Data = new List<CollectedData<String>>();
            }

            public override void VisitCastExpression(CastExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                TypeSyntax typeSyntax = node.Type;
                ExpressionSyntax expressionSyntax = node.Expression;
                TypeInfo typeModel = _model.GetTypeInfo(typeSyntax);
                TypeInfo expressionTypeModel = _model.GetTypeInfo(expressionSyntax);
                if (typeModel.Equals(expressionTypeModel))
                    Data.Add(new CollectedData<String>(typeModel.Type.ToDisplayString(), span.StartLinePosition, span.EndLinePosition));
                base.VisitCastExpression(node);
            }

            public IList<CollectedData<String>> Data { get; }

            private readonly SemanticModel _model;
        }
    }
}
