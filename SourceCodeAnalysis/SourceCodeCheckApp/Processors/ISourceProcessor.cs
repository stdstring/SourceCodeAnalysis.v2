using SourceCodeCheckApp.Analyzers;

namespace SourceCodeCheckApp.Processors
{
    public interface ISourceProcessor
    {
        Boolean Process(IList<IFileAnalyzer> analyzers);
    }
}