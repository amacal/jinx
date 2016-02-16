using Jinx.Dom;
using System;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonUniqueItemsRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            HashSet<JsonValue> found = new HashSet<JsonValue>();
            List<JsonValue> repeated = new List<JsonValue>();

            foreach (JsonValue item in target.Items())
                if (found.Add(item) == false)
                    repeated.Add(item);

            if (repeated.Count > 0)
            {
                string items = String.Join(",", repeated);
                string message = $"The array elements should be unique. Repeated elements: [{items}]";

                return callback.Fail(value, message);
            }

            return true;
        }
    }
}