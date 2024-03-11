using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NonAsciiIdentifiersAnalyzer : IFileAnalyzer
    {
        public NonAsciiIdentifiersAnalyzer(OutputImpl output)
        {
            _output = output;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            _output.WriteInfoLine($"Execution of NonAsciiIdentifiersAnalyzer started");
            Regex identifierRegex = new Regex("^[a-zA-Z0-9_]+$");
            NonConsistentIdentifiersDetector detector = new NonConsistentIdentifiersDetector(identifierRegex);
            detector.Visit(tree.GetRoot());
            Boolean hasErrors = ProcessErrors(filePath, detector.Data);
            _output.WriteInfoLine($"Execution of NonAsciiIdentifiersAnalyzer finished");
            return !hasErrors;
        }

        private Boolean ProcessErrors(String filePath, IList<CollectedData<String>> errors)
        {
            _output.WriteInfoLine($"Found {errors.Count} non-ASCII identifiers leading to errors in the ported C++ code");
            foreach (CollectedData<String> error in errors)
                _output.WriteErrorLine(filePath, error.StartPosition.Line, $"Found non-ASCII identifier \"{error.Data}\"");
            return errors.Count > 0;
        }

        private readonly OutputImpl _output;

        private class NonConsistentIdentifiersDetector : CSharpSyntaxWalker
        {
            public NonConsistentIdentifiersDetector(Regex identifierRegex) : base(SyntaxWalkerDepth.Token)
            {
                Data = new List<CollectedData<String>>();
                _identifierRegex = identifierRegex;
            }

            public override void VisitToken(SyntaxToken token)
            {
                FileLinePositionSpan span = token.SyntaxTree.GetLineSpan(token.Span);
                if (token.Kind() == SyntaxKind.IdentifierToken && !_identifierRegex.IsMatch(token.ValueText))
                    Data.Add(new CollectedData<String>(token.ValueText, span.StartLinePosition, span.EndLinePosition));
                base.VisitToken(token);
            }

            public IList<CollectedData<String>> Data { get; }

            private readonly Regex _identifierRegex;
        }
    }
}
