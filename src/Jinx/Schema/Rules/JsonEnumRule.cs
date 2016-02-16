using Jinx.Dom;
using System;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonEnumRule : JsonSchemaRule
    {
        private readonly HashSet<JsonValue> values;

        public JsonEnumRule()
        {
            this.values = new HashSet<JsonValue>();
        }

        public void Add(JsonValue value)
        {
            values.Add(value);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            if (values.Contains(value))
                return true;

            string list = String.Join(",", values);
            string message = $"The value should be from the given list: [{list}]";

            return callback.Fail(value, message);
        }
    }
}