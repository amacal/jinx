using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchema
    {
        private readonly JsonSchemaRule rule;
        private readonly JsonSchemaDefinitions definitions;

        public JsonSchema(JsonSchemaRule rule)
        {
            this.rule = rule;
            this.definitions = new JsonSchemaDefinitions();
        }

        public bool IsValid(JsonDocument document)
        {
            return rule.IsValid(definitions, document.Root);
        }
    }
}