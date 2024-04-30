using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class NullCoalescingOperatorAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.NullCoalescingOperatorAnalyzer";

        public NullCoalescingOperatorAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            NullCoalescingOperatorDetector detector = new NullCoalescingOperatorDetector();
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} null-coalescing operators");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData<String> entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, $"Found null-coalescing operator: \"{entry.Data}\"");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class NullCoalescingOperatorDetector : CSharpSyntaxWalker
        {
            public override void VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (!node.IsKind(SyntaxKind.CoalesceExpression))
                    return;
                Data.Add(new AnalyzerData<String>("??", span));
            }

            public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (!node.IsKind(SyntaxKind.CoalesceAssignmentExpression))
                    return;
                Data.Add(new AnalyzerData<String>("??=", span));
            }

            public IList<AnalyzerData<String>> Data { get; } = new List<AnalyzerData<String>>();
        }
    }
}
