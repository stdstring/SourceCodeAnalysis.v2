using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class BadFilenameCaseAnalyzer : IFileAnalyzer
    {
        public BadFilenameCaseAnalyzer(OutputImpl output)
        {
            _output = output;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteInfoLine($"Execution of BadFilenameCaseAnalyzer started");
            TopLevelTypeNamesCollector collector = new TopLevelTypeNamesCollector(model);
            collector.Visit(tree.GetRoot());
            Boolean result = Process(filePath, collector.Data);
            _output.WriteInfoLine($"Execution of BadFilenameCaseAnalyzer finished");
            return result;
        }

        private Boolean Process(String filePath, IList<CollectedData<String>> data)
        {
            String filename = Path.GetFileName(filePath);
            String expectedTypeName = Path.GetFileNameWithoutExtension(filePath);
            IList<CollectedData<String>> typeWrongNameCaseList = new List<CollectedData<String>>();
            Boolean exactMatch = false;
            foreach (CollectedData<String> item in data)
            {
                String actualTypeName = item.Data.Split('.').Last();
                if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCulture))
                    exactMatch = true;
                else if (String.Equals(expectedTypeName, actualTypeName, StringComparison.InvariantCultureIgnoreCase))
                    typeWrongNameCaseList.Add(item);
            }
            if (!exactMatch && typeWrongNameCaseList.Count == 0)
            {
                _output.WriteWarningLine(filePath, 0, "File doesn't contain any types with names corresponding to the name of this file");
                return true;
            }
            if (!exactMatch && typeWrongNameCaseList.Count > 0)
            {
                _output.WriteInfoLine($"File doesn't contain any type with name exact match to the filename, but contains {typeWrongNameCaseList.Count} types with names match to the filename with ignoring case");
                foreach (CollectedData<String> typeWrongNameCase in typeWrongNameCaseList)
                    _output.WriteErrorLine(filePath, typeWrongNameCase.StartPosition.Line, $"Found type named \"{typeWrongNameCase.Data}\" which corresponds the filename \"{filename}\" only at ignoring case");
                return false;
            }
            _output.WriteInfoLine($"File contains {typeWrongNameCaseList.Count} types with names match to the filename with ignoring case");
            foreach (CollectedData<String> typeWrongNameCase in typeWrongNameCaseList)
                _output.WriteWarningLine(filePath, typeWrongNameCase.StartPosition.Line, $"Found type named \"{typeWrongNameCase.Data}\" which corresponds the filename \"{filename}\" only at ignoring case");
            return true;
        }

        private readonly OutputImpl _output;

        internal class TopLevelTypeNamesCollector : CSharpSyntaxWalker
        {
            public TopLevelTypeNamesCollector(SemanticModel model)
            {
                _model = model;
                Data = new List<CollectedData<String>>();
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
                base.VisitClassDeclaration(node);
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
                base.VisitStructDeclaration(node);
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
                base.VisitInterfaceDeclaration(node);
            }

            public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
                base.VisitDelegateDeclaration(node);
            }

            private void VisitTypeDeclarationImpl(MemberDeclarationSyntax node, INamedTypeSymbol type)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (type.ContainingType == null)
                    Data.Add(new CollectedData<String>(type.ToDisplayString(), span.StartLinePosition, span.EndLinePosition));
            }

            public IList<CollectedData<String>> Data { get; }

            private readonly SemanticModel _model;
        }
    }
}
