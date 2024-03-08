namespace SourceCodeCheckApp.Args
{
    internal enum AppUsageMode
    {
        Unknown = 0,
        Analysis = 1,
        Help = 2,
        Version = 3,
        BadSource = 253,
        BadConfig = 254,
        BadAppUsage = 255
    }
}