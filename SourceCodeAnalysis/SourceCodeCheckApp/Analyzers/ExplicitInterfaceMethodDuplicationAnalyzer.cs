using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class ExplicitInterfaceMethodDuplicationAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ExplicitInterfaceMethodDuplicationAnalyzer";

        public ExplicitInterfaceMethodDuplicationAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            ExplicitInterfaceMethodDuplicationDetector detector = new ExplicitInterfaceMethodDuplicationDetector(model);
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} explicit implementations of an interface with a private method of the same name");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData<String> entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, $"Found explicit implementation of an interface with a private method of the same name: {entry.Data}");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class ExplicitInterfaceMethodDuplicationDetector : CSharpSyntaxWalker
        {
            public ExplicitInterfaceMethodDuplicationDetector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                INamedTypeSymbol? typeDeclaration = _model.GetDeclaredSymbol(node);
                if (typeDeclaration == null)
                    throw new InvalidOperationException($"Bad semantic info for type: {node.Identifier}");
                IMethodSymbol[] methods = typeDeclaration.GetMembers().OfType<IMethodSymbol>().ToArray();
                IEnumerable<IMethodSymbol> privateMethods = methods
                    .Where(method => method is {MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Private});
                // TODO (std_string) : think about matching not only by name, but by arguments
                ISet<String> privateNames = privateMethods.Select(method => method.Name).ToHashSet();
                foreach (MethodDeclarationSyntax methodDeclaration in node.Members.OfType<MethodDeclarationSyntax>())
                {
                    if (methodDeclaration.ExplicitInterfaceSpecifier == null)
                        continue;
                    IMethodSymbol? methodSymbol = _model.GetDeclaredSymbol(methodDeclaration);
                    if (methodSymbol == null)
                        throw new InvalidOperationException($"Bad semantic info for method: {typeDeclaration.ToDisplayString()}.{node.Identifier}");
                    if (methodSymbol.ExplicitInterfaceImplementations.Length == 0)
                        throw new InvalidOperationException($"Bad explicit interface impl info for method: {typeDeclaration.ToDisplayString()}.{node.Identifier}");
                    String methodName = methodSymbol.ExplicitInterfaceImplementations[0].Name;
                    if (!privateNames.Contains(methodName))
                        continue;
                    FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(methodDeclaration.Span);
                    Data.Add(new AnalyzerData<String>(methodSymbol.ToDisplayString(), span));
                }
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly SemanticModel _model;
        }
    }
}
