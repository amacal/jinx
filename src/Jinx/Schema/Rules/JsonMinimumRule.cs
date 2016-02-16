using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMinimumRule : JsonSchemaRule
    {
        private readonly decimal minimum;
        private readonly bool exclusiveMinimum;

        public JsonMinimumRule(decimal minimum, bool exclusiveMinimum)
        {
            this.minimum = minimum;
            this.exclusiveMinimum = exclusiveMinimum;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            if (exclusiveMinimum == false && target.ToDecimal() >= minimum)
                return true;

            if (exclusiveMinimum == true && target.ToDecimal() > minimum)
                return true;

            if (exclusiveMinimum == false)
                return callback.Fail(value, $"The number should be greater than {minimum}.");

            return callback.Fail(value, $"The number should be greater than or equal to {minimum}.");
        }
    }
}