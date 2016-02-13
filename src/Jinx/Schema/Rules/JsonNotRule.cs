using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonNotRule : JsonSchemaRule
    {
        private readonly JsonSchemaRule rule;

        public JsonNotRule(JsonSchemaRule rule)
        {
            this.rule = rule;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            if (rule.IsValid(definitions, value, JsonSchemaCallback.Ignore()) == false)
                return true;

            return callback.Call(value, "The schema should not be valid.");
        }
    }
}