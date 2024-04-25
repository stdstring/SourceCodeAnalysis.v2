using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal class AnalyserOutputWrapper : IOutput
    {
        public AnalyserOutputWrapper(IOutput output, AnalyzerState analyzerState)
        {
            _output = output;
            _analyzerState = analyzerState;
        }

        public void WriteInfoLine(String value)
        {
            if (_analyzerState == AnalyzerState.Off)
                return;
            _output.WriteInfoLine(value);
        }

        public void WriteInfoLine(String filename, Int32 line, String value)
        {
            if (_analyzerState == AnalyzerState.Off)
                return;
            _output.WriteInfoLine(filename, line, value);
        }

        public void WriteWarningLine(String value)
        {
            if (_analyzerState == AnalyzerState.Off)
                return;
            _output.WriteWarningLine(value);
        }

        public void WriteWarningLine(String filename, Int32 line, String value)
        {
            if (_analyzerState == AnalyzerState.Off)
                return;
            _output.WriteWarningLine(filename, line, value);
        }

        public void WriteErrorLine(String value)
        {
            switch (_analyzerState)
            {
                case AnalyzerState.On:
                    _output.WriteErrorLine(value);
                    break;
                case AnalyzerState.ErrorAsWarning:
                    _output.WriteWarningLine(value);
                    break;
            }
        }

        public void WriteErrorLine(String filename, Int32 line, String value)
        {
            switch (_analyzerState)
            {
                case AnalyzerState.On:
                    _output.WriteErrorLine(filename, line, value);
                    break;
                case AnalyzerState.ErrorAsWarning:
                    _output.WriteWarningLine(filename, line, value);
                    break;
            }
        }

        public void WriteFailLine(String value)
        {
            _output.WriteFailLine(value);
        }

        public void WriteFailLine(String filename, Int32 line, String value)
        {
            _output.WriteFailLine(filename, line, value);
        }

        private readonly IOutput _output;
        private readonly AnalyzerState _analyzerState;
    }
}
