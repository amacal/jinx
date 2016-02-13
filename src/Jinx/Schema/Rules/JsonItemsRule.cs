using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonItemsRule : JsonSchemaRule
    {
        private readonly JsonSchemaRule schema;
        private readonly List<JsonSchemaRule> tuples;

        public JsonItemsRule()
        {
            this.tuples = new List<JsonSchemaRule>();
        }

        public JsonItemsRule(JsonSchemaRule schema)
        {
            this.schema = schema;
        }

        public void AddTuple(JsonSchemaRule tuple)
        {
            tuples.Add(tuple);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (schema != null)
                foreach (JsonValue item in target.Items())
                    if (schema.IsValid(definitions, item, callback) == false)
                        return callback.Call($"[{target.IndexOf(item)}]", value, "The array element is not valid according to the item schema.");

            if (tuples != null)
                for (int i = 0; i < tuples.Count && i < target.Count; i++)
                    if (tuples[i].IsValid(definitions, target.Get(i), callback) == false)
                        return callback.Call($"[{i}]", value, "The array element is not valid according to the tuple schema.");

            return true;
        }
    }
}