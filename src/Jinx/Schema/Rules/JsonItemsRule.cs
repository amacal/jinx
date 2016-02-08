using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonItemsRule : JsonSchemaRule
    {
        private readonly JsonSchemaRule schema;

        public JsonItemsRule(JsonSchemaRule schema)
        {
            this.schema = schema;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            foreach (JsonValue item in target.Items())
                if (schema.IsValid(definitions, item, callback) == false)
                    return false;

            return true;
        }
    }
}