using Jinx.Dom;
using Jinx.Schema;
using System.Collections.Generic;
using System.IO;
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
        public void ValidateDocumentAgainstSchema(string schemaPath, string documentPath)
        {
            using (TextReader schemaReader = OpenReader(schemaPath))
            using (TextReader documentReader = OpenReader(documentPath))
            {
                JsonSchema schema = JsonConvert.GetSchema(schemaReader);
                JsonDocument document = JsonConvert.GetDocument(documentReader);

                Assert.True(schema.IsValid(document.Root));
            }
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