using Jinx.Dom;
using Jinx.Path;
using Jinx.Path.Segments;
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
            bool succeeded = true;
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (schema != null)
            {
                foreach (JsonValue item in target.Items())
                {
                    int index = target.IndexOf(item);
                    JsonPathSegment segment = new JsonIndexSegment(index);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    if (schema.IsValid(definitions, item, scope) == false)
                    {
                        callback.Add(scope);
                        succeeded = false;
                    }
                }
            }

            if (tuples != null)
            {
                for (int i = 0; i < tuples.Count && i < target.Count; i++)
                {
                    JsonPathSegment segment = new JsonIndexSegment(i);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    JsonValue item = target.Get(i);
                    JsonSchemaRule rule = tuples[i];

                    if (rule.IsValid(definitions, item, scope) == false)
                    {
                        callback.Add(scope);
                        succeeded = false;
                    }
                }
            }

            return succeeded;
        }
    }
}