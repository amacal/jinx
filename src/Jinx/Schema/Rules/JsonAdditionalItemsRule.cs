using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonAdditionalItemsRule : JsonSchemaRule
    {
        private readonly int items;

        public JsonAdditionalItemsRule(int items)
        {
            this.items = items;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (target.Count <= items)
                return true;

            return callback.Call(value, "The array should not contain additional items.");
        }
    }
}