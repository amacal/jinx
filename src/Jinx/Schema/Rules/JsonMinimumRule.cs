﻿using Jinx.Dom;
using System;

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

            if (exclusiveMinimum == false && Decimal.Parse(target.Value) >= minimum)
                return true;

            if (exclusiveMinimum == true && Decimal.Parse(target.Value) > minimum)
                return true;

            if (exclusiveMinimum == false)
                return callback.Call(value, $"The number should be greater than {minimum}.");

            return callback.Call(value, $"The number should be greater than or equal to {minimum}.");
        }
    }
}