using Jinx.Dom;
using Jinx.Schema;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Extensions;

namespace Jinx.Tests
{
    public class JsonConvertTests
    {
        [Fact]
        public void ParseResourceToJsonDocument()
        {
            using (TextReader reader = OpenReader("flickr.json"))
            {
                Assert.NotNull(JsonConvert.GetDocument(reader));
            }
        }

        [Fact]
        public void ParseResourceToJsonSchema()
        {
            using (TextReader reader = OpenReader("appaloosa.application-upload-schema.json"))
            {
                Assert.NotNull(JsonConvert.GetSchema(reader));
            }
        }

        [Theory]
        [MemberData("Amazon")]
        [MemberData("Appaloosa")]
        public void ValidateDocumentAgainstSchema(string schemaPath, string documentPath)
        {
            using (TextReader schemaReader = OpenReader(schemaPath))
            using (TextReader documentReader = OpenReader(documentPath))
            {
                JsonSchema schema = JsonConvert.GetSchema(schemaReader);
                JsonDocument document = JsonConvert.GetDocument(documentReader);

                Assert.True(schema.IsValid(document));
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