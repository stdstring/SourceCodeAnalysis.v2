using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Analyzers
{
    internal abstract class SimpleAnalyzerBase : IFileAnalyzer
    {
        public SimpleAnalyzerBase(IOutput output, AnalyzerState analyzerState, String name, String description)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
            _name = name;
            AnalyzerInfo = new AnalyzerInfo(name, description);
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {_name} started");
            IList<AnalyzerData> entries = Detect(tree.GetRoot(), model);
            _output.WriteInfoLine(CreateSummary(entries.Count));
            foreach (AnalyzerData entry in entries)
                _output.WriteErrorLine(filePath, entry.StartPosition.Line, CreateEntry(entry));
            _output.WriteInfoLine($"Execution of {_name} finished");
            return (_analyzerState != AnalyzerState.On) || entries.IsEmpty();
        }

        public AnalyzerInfo AnalyzerInfo { get; init; }

        protected abstract IList<AnalyzerData> Detect(SyntaxNode node, SemanticModel model);

        protected abstract String CreateSummary(Int32 entryCount);

        protected abstract String CreateEntry(AnalyzerData entry);

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;
        private readonly String _name;
    }

    internal abstract class SimpleAnalyzerBase<T> : IFileAnalyzer
    {
        public SimpleAnalyzerBase(IOutput output, AnalyzerState analyzerState, String name, String description)
        {
            _output = new AnalyserOutputWrapper(output, analyzerState);
            _analyzerState = analyzerState;
            _name = name;
            AnalyzerInfo = new AnalyzerInfo(name, description);
        }

        public Boolean Process(String filePath, SyntaxTree tree, SemanticModel model)
        {
            if (_analyzerState == AnalyzerState.Off)
                return true;
            _output.WriteInfoLine($"Execution of {_name} started");
            IList<AnalyzerData<T>> entries = Detect(tree.GetRoot(), model);
            _output.WriteInfoLine(CreateSummary(entries.Count));
            foreach (AnalyzerData<T> entry in entries)
                _output.WriteErrorLine(filePath, entry.StartPosition.Line, CreateEntry(entry));
            _output.WriteInfoLine($"Execution of {_name} finished");
            return (_analyzerState != AnalyzerState.On) || entries.IsEmpty();
        }

        public AnalyzerInfo AnalyzerInfo { get; init; }

        protected abstract IList<AnalyzerData<T>> Detect(SyntaxNode node, SemanticModel model);

        protected abstract String CreateSummary(Int32 entryCount);

        protected abstract String CreateEntry(AnalyzerData<T> entry);

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;
        private readonly String _name;
    }
}
