namespace SourceCodeCheckApp.Processors
{
    internal static class ProjectIgnoredFiles
    {
        public static Boolean IgnoreFile(String filename)
        {
            return KnownIgnoredFilesSuffix.Any(filename.EndsWith);
        }

        // TODO (std_string) : think about another approach (via config)
        private static readonly String[] KnownIgnoredFilesSuffix =
        {
            ".AssemblyInfo.cs",
            ".AssemblyAttributes.cs",
            ".GlobalUsings.g.cs"
        };
    }
}
