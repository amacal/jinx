using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMinItemsRule : JsonSchemaRule
    {
        private readonly int minItems;

        public JsonMinItemsRule(int minItems)
        {
            this.minItems = minItems;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (minItems <= target.Count)
                return true;

            return callback.Call(value, $"The array should have more than or equal to {minItems} items");
        }
    }
}