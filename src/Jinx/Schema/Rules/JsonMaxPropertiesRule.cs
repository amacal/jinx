using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMaxPropertiesRule : JsonSchemaRule
    {
        private readonly int maxProperties;

        public JsonMaxPropertiesRule(int maxProperties)
        {
            this.maxProperties = maxProperties;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            if (maxProperties < target.Count)
                return false;

            return true;
        }
    }
}