using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonMultipleOfRule : JsonSchemaRule
    {
        private readonly decimal multipleOf;

        public JsonMultipleOfRule(decimal multipleOf)
        {
            this.multipleOf = multipleOf;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            decimal division = target.ToDecimal() / multipleOf;
            long integer = Convert.ToInt64(division);

            if (integer == division)
                return true;

            return callback.Fail(value, $"The number should be multiplication of {multipleOf}.");
        }
    }
}