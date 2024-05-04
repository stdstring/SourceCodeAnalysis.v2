using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class AutoImplPropertiesAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.AutoImplPropertiesAnalyzer";

        public AutoImplPropertiesAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            AutoImplPropertiesDetector detector = new AutoImplPropertiesDetector(model);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} auto implemented properties";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found auto implemented property: {entry.Data}";
        }

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
