﻿using Jinx.Dom;
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

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            if (values.Contains(value))
                return true;

            return false;
        }
    }
}