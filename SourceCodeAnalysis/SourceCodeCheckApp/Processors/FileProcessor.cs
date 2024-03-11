using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp.Processors
{
    internal class FileProcessor : ISourceProcessor
    {
        public FileProcessor(String filename, OutputImpl output)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));
            _filename = filename;
            _output = output;
            _processorHelper = new FileProcessorHelper();
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            _output.WriteInfoLine($"Processing of the file {_filename} is started");
            if (!File.Exists(_filename))
            {
                _output.WriteFailLine($"Bad (unknown) target {_filename}");
                return false;
            }
            String source = File.ReadAllText(_filename);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            Compilation compilation = CreateCompilation(tree);
            if (!CompilationChecker.CheckCompilationErrors(_filename, compilation, _output))
                return false;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = _processorHelper.Process(_filename, tree, model, analyzers);
            _output.WriteInfoLine($"Processing of the file {_filename} is finished");
            return result;
        }

        private CSharpCompilation CreateCompilation(SyntaxTree tree)
        {
            String assemblyName = Path.GetFileNameWithoutExtension(_filename);
            return CSharpCompilation.Create(assemblyName)
                // mscorlib
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                // System.Core.dll
                .AddReferences(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        private readonly String _filename;
        private readonly OutputImpl _output;
        private readonly FileProcessorHelper _processorHelper;
    }
}