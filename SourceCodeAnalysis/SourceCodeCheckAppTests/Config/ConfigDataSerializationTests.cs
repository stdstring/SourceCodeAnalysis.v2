using NUnit.Framework;
using SourceCodeCheckApp.Config;
using System.Xml.Serialization;

namespace SourceCodeCheckAppTests.Config
{
    [TestFixture]
    public class ConfigDataSerializationTests
    {
        [Test]
        public void DeserializeWithBaseConfigOnly()
        {
            const String source = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "<Config>\r\n" +
                                  "  <BaseConfig>\r\n" +
                                  "    <Source>..\\source\\someproj.csproj</Source>\r\n" +
                                  "    <OutputLevel>Warning</OutputLevel>\r\n" +
                                  "  </BaseConfig>\r\n" +
                                  "</Config>";
            ConfigData expected = new ConfigData
            {
                BaseConfig = new BaseConfig
                {
                    Source = "..\\source\\someproj.csproj",
                    OutputLevel = OutputLevel.Warning
                },
                Analyzers = Array.Empty<AnalyzerEntry>()
            };
            CheckDeserialization(expected, source);
        }

        [Test]
        public void DeserializeWithBaseConfigOnlyWithDefaultOutputLevel()
        {
            const String source = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "<Config>\r\n" +
                                  "  <BaseConfig>\r\n" +
                                  "    <Source>..\\source\\someproj.csproj</Source>\r\n" +
                                  "  </BaseConfig>\r\n" +
                                  "</Config>";
            ConfigData expected = new ConfigData
            {
                BaseConfig = new BaseConfig
                {
                    Source = "..\\source\\someproj.csproj",
                    OutputLevel = OutputLevel.Error
                },
                Analyzers = Array.Empty<AnalyzerEntry>()
            };
            CheckDeserialization(expected, source);
        }

        [Test]
        public void DeserializeWithBaseConfigWithAnalyzers()
        {
            const String source = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "<Config>\r\n" +
                                  "  <BaseConfig>\r\n" +
                                  "    <Source>..\\source\\someproj.csproj</Source>\r\n" +
                                  "    <OutputLevel>Warning</OutputLevel>\r\n" +
                                  "  </BaseConfig>\r\n" +
                                  "  <Analyzers>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer</Name>\r\n" +
                                  "      <State>Off</State>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer</Name>\r\n" +
                                  "      <State>On</State>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer</Name>\r\n" +
                                  "      <State>ErrorAsWarning</State>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "  </Analyzers>\r\n" +
                                  "</Config>";
            ConfigData expected = new ConfigData
            {
                BaseConfig = new BaseConfig
                {
                    Source = "..\\source\\someproj.csproj",
                    OutputLevel = OutputLevel.Warning
                },
                Analyzers = new []
                {
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer", State = AnalyzerState.Off},
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer", State = AnalyzerState.On},
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer", State = AnalyzerState.ErrorAsWarning}
                }
            };
            CheckDeserialization(expected, source);
        }

        [Test]
        public void DeserializeWithBaseConfigWithAnalyzersWithDefaultState()
        {
            const String source = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "<Config>\r\n" +
                                  "  <BaseConfig>\r\n" +
                                  "    <Source>..\\source\\someproj.csproj</Source>\r\n" +
                                  "    <OutputLevel>Warning</OutputLevel>\r\n" +
                                  "  </BaseConfig>\r\n" +
                                  "  <Analyzers>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer</Name>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer</Name>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "    <Analyzer>\r\n" +
                                  "      <Name>SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer</Name>\r\n" +
                                  "    </Analyzer>\r\n" +
                                  "  </Analyzers>\r\n" +
                                  "</Config>";
            ConfigData expected = new ConfigData
            {
                BaseConfig = new BaseConfig
                {
                    Source = "..\\source\\someproj.csproj",
                    OutputLevel = OutputLevel.Warning
                },
                Analyzers = new[]
                {
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer", State = AnalyzerState.Off},
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer", State = AnalyzerState.Off},
                    new AnalyzerEntry{Name = "SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer", State = AnalyzerState.Off}
                }
            };
            CheckDeserialization(expected, source);
        }

        private void CheckDeserialization(ConfigData expected, String actualSource)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            using (StringReader reader = new StringReader(actualSource))
            {
                ConfigData? actual = serializer.Deserialize(reader) as ConfigData;
                Assert.That(actual, Is.Not.Null);
                CheckConfigData(expected, actual!);
            }
        }

        private void CheckConfigData(ConfigData expected, ConfigData actual)
        {
            Assert.That(actual.BaseConfig, Is.Not.Null);
            CheckBaseConfig(expected.BaseConfig!, actual.BaseConfig!);
            CheckAnalyzers(expected.Analyzers!, actual.Analyzers);
        }

        private void CheckBaseConfig(BaseConfig expected, BaseConfig actual)
        {
            Assert.That(actual.Source, Is.EqualTo(expected.Source));
            Assert.That(actual.OutputLevel, Is.EqualTo(expected.OutputLevel));
        }

        private void CheckAnalyzers(AnalyzerEntry[] expected, AnalyzerEntry[]? actual)
        {
            if (expected.Length == 0)
                Assert.That(actual == null || actual.Length == 0);
            else
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual!.Length, Is.EqualTo(expected.Length));
                for (Int32 index = 0; index < expected.Length; ++index)
                    CheckAnalyzer(expected[index], actual[index]);
            }
        }

        private void CheckAnalyzer(AnalyzerEntry expected, AnalyzerEntry actual)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.State, Is.EqualTo(expected.State));
        }
    }
}
