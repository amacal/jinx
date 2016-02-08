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
            JsonDocument document = GetDocument(reader);
            bool isValid = JsonSchema.Draft04.IsValid(document.Root);

            if (isValid == false)
                throw new JsonSchemaException("The schema is not valid against draft-04.");

            JsonSchemaReader schemaReader = new JsonSchemaReader(document);
            JsonSchema schema = schemaReader.Load();

            return schema;
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