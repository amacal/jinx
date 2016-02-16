using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMaxLengthRule : JsonSchemaRule
    {
        private readonly int maxLength;

        public JsonMaxLengthRule(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonText text = value as JsonText;

            if (text == null)
                return true;

            if (text.Value.Length <= maxLength)
                return true;

            return callback.Fail(value, $"The text cannot be longer than {maxLength} characters.");
        }
    }
}