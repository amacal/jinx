using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchema
    {
        private readonly JsonSchemaRule rule;
        private readonly JsonSchemaDefinitions definitions;

        public JsonSchema(JsonSchemaRule rule, JsonSchemaDefinitions definitions)
        {
            this.rule = rule;
            this.definitions = definitions;
        }

        public bool IsValid(JsonValue value)
        {
            return rule.IsValid(definitions, value);
        }
    }
}