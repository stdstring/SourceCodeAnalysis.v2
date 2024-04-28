using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal class ObjectInitializerExprAnalyzer : IFileAnalyzer
    {
        public const String Name = "SourceCodeCheckApp.Analyzers.ObjectInitializerExprAnalyzer";

        public ObjectInitializerExprAnalyzer(IOutput output, AnalyzerState analyzerState)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {Name} started");
            ObjectInitializerExprDetector detector = new ObjectInitializerExprDetector();
            detector.Visit(tree.GetRoot());
            _output.WriteInfoLine($"Found {detector.Data.Count} object initializer expressions");
            if (detector.Data.Count > 0)
            {
                foreach (AnalyzerData entry in detector.Data)
                    _output.WriteErrorLine(filePath, entry.StartPosition.Line, "Found object initializer expressions");
            }
            _output.WriteInfoLine($"Execution of {Name} finished");
            return (_analyzerState != AnalyzerState.On) || detector.Data.IsEmpty();
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;

        private class ObjectInitializerExprDetector : CSharpSyntaxWalker
        {
            public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                FileLinePositionSpan span = node.SyntaxTree.GetLineSpan(node.Span);
                if (node.Initializer is {Expressions.Count: > 0})
                    Data.Add(new AnalyzerData(span));
            }

            public IList<AnalyzerData> Data { get; } = new List<AnalyzerData>();
        }
    }
}
