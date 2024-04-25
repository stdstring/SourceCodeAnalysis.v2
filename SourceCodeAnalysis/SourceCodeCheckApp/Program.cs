using System.Text;
using SourceCodeCheckApp.Analyzers;
using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;
using SourceCodeCheckApp.Processors;
using SourceCodeCheckApp.Utils;

namespace SourceCodeCheckApp
{
    public class Program
    {
        public static Int32 Main(String[] args)
        {
            try
            {
                Boolean result = MainImpl(args);
                return result ? 0 : -1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[ERROR]: {e.Message}");
                return -1;
            }
        }

        private static Boolean MainImpl(String[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            switch (AppArgsParser.Parse(args))
            {
                case AppArgsResult.VersionConfig {Version: var version}:
                    Console.WriteLine(version);
                    return true;
                case AppArgsResult.HelpConfig {Help: var help}:
                    Console.WriteLine(help);
                    return true;
                case AppArgsResult.WrongConfig {Help: var help, Reason: var reason}:
                    Console.Error.WriteLine($"[ERROR]: {reason}");
                    Console.WriteLine(help);
                    return false;
                case AppArgsResult.MainConfig mainConfig:
                    AppConfig appConfig = AppConfigFactory.Create(mainConfig);
                    IOutput output = new OutputImpl(Console.Out, Console.Error, appConfig.Config.BaseConfig!.OutputLevel);
                    PrerequisitesManager.Run();
                    ISourceProcessor processor = SourceProcessorFactory.Create(appConfig.Config.BaseConfig.Source!, output);
                    IList<IFileAnalyzer> analyzers = AnalyzersFactory.Create(output, appConfig.Config.Analyzers ?? Array.Empty<AnalyzerEntry>());
                    Boolean processResult = processor.Process(analyzers);
                    output.WriteInfoLine($"Result of analysis: analysis is {(processResult ? "succeeded" : "failed")}");
                    return processResult;
                default:
                    throw new InvalidOperationException("Unsupported args");
            }
        }
    }
}
