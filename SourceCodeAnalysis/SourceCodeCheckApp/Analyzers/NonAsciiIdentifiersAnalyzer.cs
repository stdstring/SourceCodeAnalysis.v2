using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NonAsciiIdentifiersAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer";

        public NonAsciiIdentifiersAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of NonAsciiIdentifiersAnalyzer started");
            Regex identifierRegex = new Regex("^[a-zA-Z0-9_]+$");
            NonConsistentIdentifiersDetector detector = new NonConsistentIdentifiersDetector(identifierRegex);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(filePath, detector.Data);
            _output.WriteInfoLine($"Execution of NonAsciiIdentifiersAnalyzer finished");
            return !hasErrors;
        }

        private Boolean ProcessErrors(String filePath, IList<AnalyzerData<String>> errors)
        {
            _output.WriteInfoLine($"Found {errors.Count} non-ASCII identifiers leading to errors in the ported C++ code");
            foreach (AnalyzerData<String> error in errors)
                _output.WriteErrorLine(filePath, error.StartPosition.Line, $"Found non-ASCII identifier \"{error.Data}\"");
            return errors.Count > 0;
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class NonConsistentIdentifiersDetector : CSharpSyntaxWalker
        {
            public NonConsistentIdentifiersDetector(Regex identifierRegex) : base(SyntaxWalkerDepth.Token)
            {
                _identifierRegex = identifierRegex;
            }

            public override void VisitToken(SyntaxToken token)
            {
                SyntaxTree? syntaxTree = token.SyntaxTree;
                if (syntaxTree == null)
                    throw new InvalidOperationException();
                FileLinePositionSpan span = syntaxTree.GetLineSpan(token.Span);
                if (token.IsKind(SyntaxKind.IdentifierToken) && !_identifierRegex.IsMatch(token.ValueText))
                    Data.Add(new AnalyzerData<String>(token.ValueText, span));
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();

            private readonly Regex _identifierRegex;
        }
    }
}
