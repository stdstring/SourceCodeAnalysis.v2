namespace SourceCodeCheckApp.Processors
{
    internal static class ProjectIgnoredFiles
    {
        public static Boolean IgnoreFile(String filename)
        {
            return KnownIgnoredFilesSuffix.Any(filename.EndsWith);
        }

        private static readonly String[] KnownIgnoredFilesSuffix =
        {
            "Properties\\AssemblyInfo.cs",
            ".AssemblyAttributes.cs"
        };
    }
}
