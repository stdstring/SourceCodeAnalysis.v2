using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal record MemberData(String Kind, String FullName);

    internal class ExprBodiedMemberAnalyzer : SimpleAnalyzerBase<MemberData>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ExprBodiedMemberAnalyzer";

        public ExprBodiedMemberAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<MemberData>> Detect(SyntaxNode node, SemanticModel model)
        {
            ExprBodiedMemberDetector detector = new ExprBodiedMemberDetector(model);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} expression-bodied members";
        }

        protected override String CreateEntry(AnalyzerData<MemberData> entry)
        {
            return $"Found expression-bodied {entry.Data.Kind}: {entry.Data.FullName}";
        }

        private class ExprBodiedMemberDetector : CSharpSyntaxWalker
        {
            public ExprBodiedMemberDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.ExpressionBody == null)
                    return;
                IMethodSymbol? methodSymbol = _model.GetDeclaredSymbol(node);
                if (methodSymbol == null)
                    throw new InvalidOperationException($"Bad declaration of method: {node}");
                MemberData memberData = new MemberData("method", methodSymbol.ToDisplayString());
                Data.Add(new AnalyzerData<MemberData>(memberData, span));
            }

            public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.ExpressionBody == null)
                    return;
                IMethodSymbol? ctorSymbol = _model.GetDeclaredSymbol(node);
                if (ctorSymbol == null)
                    throw new InvalidOperationException($"Bad declaration of ctor: {node}");
                MemberData memberData = new MemberData("ctor", ctorSymbol.ToDisplayString());
                Data.Add(new AnalyzerData<MemberData>(memberData, span));
            }

            public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if ((node.ExpressionBody == null) &&
                    ((node.AccessorList == null) || (node.AccessorList.Accessors.All(accessor => accessor.ExpressionBody == null))))
                    return;
                IPropertySymbol? propertySymbol = _model.GetDeclaredSymbol(node);
                if (propertySymbol == null)
                    throw new InvalidOperationException($"Bad declaration of property: {node}");
                MemberData memberData = new MemberData("property", propertySymbol.ToDisplayString());
                Data.Add(new AnalyzerData<MemberData>(memberData, span));
            }

            public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if ((node.ExpressionBody == null) &&
                    ((node.AccessorList == null) || (node.AccessorList.Accessors.All(accessor => accessor.ExpressionBody == null))))
                    return;
                IPropertySymbol? propertySymbol = _model.GetDeclaredSymbol(node);
                if (propertySymbol == null)
                    throw new InvalidOperationException($"Bad declaration of indexer: {node}");
                MemberData memberData = new MemberData("indexer", propertySymbol.ToDisplayString());
                Data.Add(new AnalyzerData<MemberData>(memberData, span));
            }

            public IList<AnalyzerData<MemberData>> Data { get; } = new List<AnalyzerData<MemberData>>();

            private readonly SemanticModel _model;
        }
    }
}
