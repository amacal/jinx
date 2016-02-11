using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchemaReader
    {
        private readonly JsonDocument document;
        private readonly JsonSchemaParser parser;

        public JsonSchemaReader(JsonDocument document)
        {
            this.document = document;
            this.parser = new JsonSchemaParser();
        }

        public JsonSchema Load()
        {
            JsonObject root = document.Root as JsonObject;
            JsonSchemaRepository repository = new JsonSchemaRepository();

            JsonSchemaDefinitions definitions = new JsonSchemaDefinitions(repository, root);
            JsonSchema schema = new JsonSchema(parser.Parse(root), definitions);

            return schema;
        }
    }
}