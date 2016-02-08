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

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            if (rule.IsValid(definitions, value) == false)
                return true;

            return false;
        }
    }
}