using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonArrayRule : JsonSchemaRule
    {
        private JsonSchemaRule rule;

        public void SetSchema(JsonSchemaRule rule)
        {
            this.rule = rule;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonArray array = value as JsonArray;

            if (array == null)
                return false;

            if (rule != null)
                foreach (JsonValue item in array.Items())
                    if (rule.IsValid(definitions, item) == false)
                        return false;

            return true;
        }
    }
}
