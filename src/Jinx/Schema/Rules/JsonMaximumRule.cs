using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMaximumRule : JsonSchemaRule
    {
        private readonly decimal maximum;
        private readonly bool exclusiveMaximum;

        public JsonMaximumRule(decimal maximum, bool exclusiveMaximum)
        {
            this.maximum = maximum;
            this.exclusiveMaximum = exclusiveMaximum;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            if (exclusiveMaximum == false && target.ToDecimal() <= maximum)
                return true;

            if (exclusiveMaximum == true && target.ToDecimal() < maximum)
                return true;

            return callback.Call(value, $"The number should be greater than {maximum}.");
        }
    }
}