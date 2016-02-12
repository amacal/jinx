using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMinLengthRule : JsonSchemaRule
    {
        private readonly int minLength;

        public JsonMinLengthRule(int minLength)
        {
            this.minLength = minLength;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonText text = value as JsonText;

            if (text == null)
                return true;

            if (text.Value.Length >= minLength)
                return true;

            return false;
        }
    }
}