using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchemaDefinitions
    {
        private readonly JsonObject root;
        private readonly JsonSchemaParser parser;
        private readonly JsonSchemaRepository repository;

        public JsonSchemaDefinitions(JsonSchemaRepository repository, JsonObject root)
        {
            this.root = root;
            this.parser = new JsonSchemaParser();
            this.repository = repository;
        }

        public JsonSchema Resolve(string reference)
        {
            if (reference.StartsWith("#"))
            {
                string[] parts = reference.Split('/');
                JsonObject node = FindNode(parts);

                JsonSchemaRule rule = parser.Parse(node);
                JsonSchema schema = new JsonSchema(rule, this);

                return schema;
            }

            return repository.GetByReference(reference);
        }

        private JsonObject FindNode(string[] path)
        {
            JsonObject current = root;

            foreach (string segment in path)
            {
                if (segment != "#")
                {
                    current = current.Get(segment) as JsonObject;
                }

                if (current == null)
                {
                    return null;
                }
            }

            return current;
        }
    }
}