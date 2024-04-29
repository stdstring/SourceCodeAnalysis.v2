using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class AutoImplPropertiesAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.AutoImplPropertiesAnalyzer";

        public AutoImplPropertiesAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            AutoImplPropertiesDetector detector = new AutoImplPropertiesDetector(model);
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} auto implemented properties");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData<String> entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, $"Found auto implemented property: {entry.Data}");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class AutoImplPropertiesDetector : CSharpSyntaxWalker
        {
            public AutoImplPropertiesDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                IPropertySymbol? propertySymbol = _model.GetDeclaredSymbol(node);
                if (propertySymbol == null)
                    throw new InvalidOperationException($"Bad declaration of property: {node}");
                IEnumerable<IFieldSymbol> fields = propertySymbol.ContainingType.GetMembers().OfType<IFieldSymbol>();
                if (!fields.Any(field => SymbolEqualityComparer.Default.Equals(field.AssociatedSymbol, propertySymbol)))
                    return;
                Data.Add(new AnalyzerData<String>(propertySymbol.ToDisplayString(), span));
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
        }
    }
}
