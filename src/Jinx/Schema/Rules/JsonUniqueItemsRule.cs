using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonUniqueItemsRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            HashSet<JsonValue> items = new HashSet<JsonValue>();
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            foreach (JsonValue item in target.Items())
                if (items.Add(item) == false)
                    return false;

            return true;
        }
    }
}