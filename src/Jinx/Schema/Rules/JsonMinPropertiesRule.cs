using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMinPropertiesRule : JsonSchemaRule
    {
        private readonly int minProperties;

        public JsonMinPropertiesRule(int minProperties)
        {
            this.minProperties = minProperties;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            if (minProperties <= target.Count)
                return true;

            return callback.Call(value, $"The object cannot have less than {minProperties} properties.");
        }
    }
}