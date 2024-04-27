using Microsoft.CodeAnalysis;
using SourceCodeCheckApp.Analyzers;

namespace SourceCodeCheckApp.Processors
{
    internal class FileProcessor : ISourceProcessor
    {
        public FileProcessor(String filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename))
                throw new ArgumentException($"Bad (unknown) target {_filename}");
            _filename = Path.GetFullPath(filename);
        }

        public Boolean Process(IList<IFileAnalyzer> analyzers)
        {
            /*_output.WriteInfoLine($"Processing of the file {_filename} is started");
            String source = File.ReadAllText(_filename);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            Compilation compilation = CreateCompilation(tree);
            if (!CompilationChecker.CheckCompilationErrors(_filename, compilation, _output))
                return false;
            SemanticModel model = compilation.GetSemanticModel(tree);
            Boolean result = Process(tree, model, analyzers);
            _output.WriteInfoLine($"Processing of the file {_filename} is finished");
            return result;*/
            throw new NotSupportedException("Processing of single files does not support now");
        }

        public Boolean Process(SyntaxTree tree, SemanticModel model, IList<IFileAnalyzer> analyzers)
        {
            Boolean result = true;
            foreach (IFileAnalyzer analyzer in analyzers)
                result &= analyzer.Process(_filename, tree, model);
            return result;
        }

        /*private CSharpCompilation CreateCompilation(SyntaxTree tree)
        {
            String assemblyName = Path.GetFileNameWithoutExtension(_filename);
            return CSharpCompilation.Create(assemblyName)
                // mscorlib
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                // System.Core.dll
                .AddReferences(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location))
                .AddSyntaxTrees(tree)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }*/

        private readonly String _filename;
    }
}