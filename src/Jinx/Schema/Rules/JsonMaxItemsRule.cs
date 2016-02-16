using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMaxItemsRule : JsonSchemaRule
    {
        private readonly int maxItems;

        public JsonMaxItemsRule(int maxItems)
        {
            this.maxItems = maxItems;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (maxItems >= target.Count)
                return true;

            return callback.Fail(value, $"The array cannot have more than {maxItems} items.");
        }
    }
}