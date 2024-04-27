using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class DefaultLiteralAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.DefaultLiteralAnalyzer";

        public DefaultLiteralAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            DefaultLiteralDetector detector = new DefaultLiteralDetector();
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} target-typed default literals");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, "Found target-typed default literal");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class DefaultLiteralDetector : CSharpSyntaxWalker
        {
            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.IsKind(SyntaxKind.DefaultLiteralExpression))
                    Data.Add(new AnalyzerData(span));
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
