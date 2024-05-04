using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NonAsciiIdentifiersAnalyzer : SimpleAnalyzerBase<String>
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer";

        public NonAsciiIdentifiersAnalyzer(IOutput output, AnalyzerState analyzerState) : base(output, analyzerState, Name)
        {
        }

        protected override IList<AnalyzerData<String>> Detect(SyntaxNode node, SemanticModel model)
        {
            Regex identifierRegex = new Regex("^[a-zA-Z0-9_]+$");
            NonConsistentIdentifiersDetector detector = new NonConsistentIdentifiersDetector(identifierRegex);
            detector.Visit(node);
            return detector.Data;
        }

        protected override String CreateSummary(Int32 entryCount)
        {
            return $"Found {entryCount} non-ASCII identifiers leading to errors in the ported C++ code";
        }

        protected override String CreateEntry(AnalyzerData<String> entry)
        {
            return $"Found non-ASCII identifier \"{entry.Data}\"";
        }

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
