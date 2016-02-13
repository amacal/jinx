﻿using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonUniqueItemsRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;
            HashSet<JsonValue> items = new HashSet<JsonValue>();

            if (target == null)
                return true;

            foreach (JsonValue item in target.Items())
                if (items.Add(item) == false)
                    return callback.Call(value, "The array elements should be unique.");

            return true;
        }
    }
}