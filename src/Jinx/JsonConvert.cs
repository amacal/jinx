using Jinx.Dom;
using Jinx.Reader;
using Jinx.Schema;
using System.IO;

namespace Jinx
{
    public static class JsonConvert
    {
        public static JsonDocument GetDocument(TextReader reader)
        {
            var jsonReader = new JsonReader(reader);
            var documentReader = new JsonDocumentReader(jsonReader);

            return documentReader.Load();
        }

        public static JsonDocument GetDocument(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return GetDocument(reader);
            }
        }

        public static JsonDocument GetDocument(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return GetDocument(reader);
            }
        }

        public static JsonSchema GetSchema(TextReader reader)
        {
            var document = GetDocument(reader);
            var schemaReader = new JsonSchemaReader(document);

            return schemaReader.Load();
        }

        public static JsonSchema GetSchema(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return GetSchema(reader);
            }
        }
    }
}