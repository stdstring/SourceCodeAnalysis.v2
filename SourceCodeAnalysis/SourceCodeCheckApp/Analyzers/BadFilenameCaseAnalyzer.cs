using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class BadFilenameCaseAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer";

        public BadFilenameCaseAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            TopLevelTypeNamesCollector collector = new TopLevelTypeNamesCollector(model);
            collector.Visit(tree.GetRoot());
            Boolean result = Process(filePath, collector.Data);
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || result;
        }

        private Boolean Process(String filePath, IList<AnalyzerData<SimpleTypeData>> data)
        {
            String filename = Path.GetFileName(filePath);
            String expectedTypeName = Path.GetFileNameWithoutExtension(filePath);
            IList<AnalyzerData<SimpleTypeData>> typeWrongNameCaseList = new List<AnalyzerData<SimpleTypeData>>();
            Boolean exactMatch = false;
            foreach (AnalyzerData<SimpleTypeData> item in data)
            {
                (Boolean exactMatch, Boolean isWrongNameCase) typeResult = Process(expectedTypeName, item);
                exactMatch |= typeResult.exactMatch;
                if (typeResult.isWrongNameCase)
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
                foreach (AnalyzerData<SimpleTypeData> typeWrongNameCase in typeWrongNameCaseList)
                    _output.WriteErrorLine(filePath, typeWrongNameCase.StartPosition.Line, $"Found type named \"{typeWrongNameCase.Data.FullName}\" which corresponds the filename \"{filename}\" only at ignoring case");
                return false;
            }
            _output.WriteInfoLine($"File contains {typeWrongNameCaseList.Count} types with names match to the filename with ignoring case");
            foreach (AnalyzerData<SimpleTypeData> typeWrongNameCase in typeWrongNameCaseList)
                _output.WriteWarningLine(filePath, typeWrongNameCase.StartPosition.Line, $"Found type named \"{typeWrongNameCase.Data.FullName}\" which corresponds the filename \"{filename}\" only at ignoring case");
            return true;
        }

        private (Boolean exactMatch, Boolean isWrongNameCase) Process(String expectedTypeName, AnalyzerData<SimpleTypeData> entry)
        {
            Boolean exactMatch = false;
            Boolean isWrongNameCase = false;
            if (String.Equals(expectedTypeName, entry.Data.TypeName, StringComparison.InvariantCulture))
                exactMatch = true;
            else if (String.Equals(expectedTypeName, entry.Data.TypeName, StringComparison.InvariantCultureIgnoreCase))
                isWrongNameCase = true;
            return (exactMatch, isWrongNameCase);
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class TopLevelTypeNamesCollector : CSharpSyntaxWalker
        {
            public TopLevelTypeNamesCollector(SemanticModel model)
            {
                _model = model;
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
            }

            public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
            {
                VisitTypeDeclarationImpl(node, _model.GetDeclaredSymbol(node));
            }

            private void VisitTypeDeclarationImpl(MemberDeclarationSyntax node, INamedTypeSymbol? type)
            {
                if (type == null)
                    throw new InvalidOperationException();
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (type.ContainingType != null)
                    return;
                if (String.IsNullOrEmpty(type.Name))
                    throw new InvalidOperationException("Bad type declaration");
                Data.Add(new AnalyzerData<SimpleTypeData>(new SimpleTypeData(type.Name, type.ToDisplayString()), span));
            }

            public IList<AnalyzerData<SimpleTypeData>> Data { get; } = new List<AnalyzerData<SimpleTypeData>>();

            private readonly SemanticModel _model;
        }
    }
}
