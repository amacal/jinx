using Jinx.Dom;
using Jinx.Schema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

namespace Jinx.Tests
{
    public class JsonConvertTests
    {
        [Theory]
        [MemberData("Amazon")]
        [MemberData("Appaloosa")]
        [MemberData("Bel")]
        [MemberData("JsonSchema")]
        [MemberData("OpenTpx")]
        [MemberData("TrackHubRegistry")]
        [MemberData("SpaceTelescope", "spacetelescope", "ok")]
        public void ValidateDocumentAgainstSchemaSucceeds(string schemaPath, string documentPath)
        {
            using (TextReader schemaReader = OpenReader(schemaPath))
            using (TextReader documentReader = OpenReader(documentPath))
            {
                JsonSchema schema = JsonConvert.GetSchema(schemaReader);
                JsonDocument document = JsonConvert.GetDocument(documentReader);

                List<string> messages = new List<string>();
                bool positive = schema.IsValid(document.Root, messages);

                Assert.True(positive);
            }
        }

        [Theory]
        [MemberData("SpaceTelescope", "spacetelescope", "bad")]
        public void ValidateDocumentAgainstSchemaFails(string schemaPath, string documentPath)
        {
            using (TextReader schemaReader = OpenReader(schemaPath))
            using (TextReader documentReader = OpenReader(documentPath))
            {
                JsonSchema schema = JsonConvert.GetSchema(schemaReader);
                JsonDocument document = JsonConvert.GetDocument(documentReader);

                Assert.False(schema.IsValid(document.Root));
            }
        }

        public static IEnumerable<object[]> SpaceTelescope(string resource, string type)
        {
            Regex schema = new Regex($@"(?<id>[0-9]{{2}})\.schema\.json$");
            Regex sample = new Regex($@"(?<id>[0-9]{{2}})\.sample\.[0-9]{{2}}\.{type}$");

            Assembly assembly = typeof(JsonConvertTests).Assembly;
            string[] resources = assembly.GetManifestResourceNames();

            foreach (var schemaEntry in resources.Select(x => new { Match = schema.Match(x), Path = x }).Where(x => x.Match.Success))
            {
                foreach (var sampleEntry in resources.Select(x => new { Match = sample.Match(x), Path = x }).Where(x => x.Match.Success))
                {
                    if (sampleEntry.Match.Groups["id"].Value == schemaEntry.Match.Groups["id"].Value)
                    {
                        if (sampleEntry.Path.Contains(resource) && schemaEntry.Path.Contains(resource))
                        {
                            yield return new string[]
                            {
                                schemaEntry.Path.Substring(21),
                                sampleEntry.Path.Substring(21)
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> SpacetelescopeSuccess()
        {
            yield return new string[]
            {
                "spacetelescope.about.01.schema.json",
                "spacetelescope.about.01.sample-02.json"
            };
        }

        public static IEnumerable<object[]> Appaloosa
        {
            get
            {
                yield return new string[]
                {
                    "appaloosa.application-upload-schema.json",
                    "appaloosa.application-upload-sample.json"
                };
            }
        }

        public static IEnumerable<object[]> OpenTpx
        {
            get
            {
                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-bgp-nc.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-collections-nc.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-countrycodes.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-emailobservable.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-ip-observables-nc.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-malware-report-2-nc.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-malware-report-nc.json"
                };

                yield return new string[]
                {
                    "open_tpx.tpx.2.2.schema.json",
                    "open_tpx.tpx2-2-example-pcap-observables-nc.json"
                };
            }
        }

        public static IEnumerable<object[]> Bel
        {
            get
            {
                yield return new string[]
                {
                    "bel.test-network-schema.json",
                    "bel.test-network-sample.json"
                };
            }
        }

        public static IEnumerable<object[]> TrackHubRegistry
        {
            get
            {
                yield return new string[]
                {
                    "trackhub_registry.schema.json",
                    "trackhub_registry.blueprint1.json"
                };

                yield return new string[]
                {
                    "trackhub_registry.schema.json",
                    "trackhub_registry.blueprint2.json"
                };
            }
        }

        public static IEnumerable<object[]> JsonSchema
        {
            get
            {
                yield return new string[]
                {
                    "json_schema.example-schema.json",
                    "json_schema.example-sample.json"
                };

                yield return new string[]
                {
                    "json_schema.self.json",
                    "json_schema.self.json"
                };
            }
        }

        public static IEnumerable<object[]> Amazon
        {
            get
            {
                yield return new string[]
                {
                    "amazon.sales_invoice.original-data-schema.json",
                    "amazon.sales_invoice.original-data-sample.json"
                };

                yield return new string[]
                {
                    "amazon.photos.original-data-schema.json",
                    "amazon.photos.original-data-sample.json"
                };

                yield return new string[]
                {
                    "amazon.news_article.original-data-schema.json",
                    "amazon.news_article.original-data-sample.json"
                };

                yield return new string[]
                {
                    "amazon.employee_record.original-data-schema.json",
                    "amazon.employee_record.original-data-sample.json"
                };
            }
        }

        private TextReader OpenReader(string resource)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), "Resources." + resource);
            TextReader reader = new StreamReader(stream);

            return reader;
        }
    }
}