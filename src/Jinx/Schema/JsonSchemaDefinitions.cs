using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchemaDefinitions
    {
        private readonly JsonObject definition;
        private readonly JsonSchemaParser parser;

        public JsonSchemaDefinitions(JsonObject definition)
        {
            this.definition = definition;
            this.parser = new JsonSchemaParser();
        }

        public JsonSchema Resolve(string reference)
        {
            string[] parts = reference.Split('/');
            JsonObject node = FindNode(parts);

            JsonSchemaRule rule = parser.Parse(node);
            JsonSchema schema = new JsonSchema(rule, this);

            return schema;
        }

        private JsonObject FindNode(string[] path)
        {
            JsonObject current = definition;

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