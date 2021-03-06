﻿using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonAllOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> rules;

        public JsonAllOfRule()
        {
            this.rules = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule rule)
        {
            rules.Add(rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            bool succeeded = true;

            foreach (JsonSchemaRule rule in rules)
                if (rule.IsValid(definitions, value, callback) == false)
                    succeeded = false;

            return succeeded;
        }
    }
}