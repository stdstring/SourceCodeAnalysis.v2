using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class ExprBodiedMemberAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ExprBodiedMemberAnalyzer";

        public ExprBodiedMemberAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            ExprBodiedMemberDetector detector = new ExprBodiedMemberDetector(model);
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} expression-bodied members");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData<MemberData> entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, $"Found expression-bodied {entry.Data.Kind}: {entry.Data.FullName}");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private record MemberData(String Kind, String FullName);

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
