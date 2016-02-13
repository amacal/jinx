using Jinx.Dom;
using System;

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

            if (exclusiveMaximum == false && Decimal.Parse(target.Value) <= maximum)
                return true;

            if (exclusiveMaximum == true && Decimal.Parse(target.Value) < maximum)
                return true;

            return callback.Call(value, $"The number should be greater than {maximum}.");
        }
    }
}