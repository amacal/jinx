using Jinx.Dom;
using Jinx.Schema.Rules;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonSchemaParser
    {
        private class CombinedRule : JsonSchemaRule
        {
            private readonly List<JsonSchemaRule> rules;

            public CombinedRule()
            {
                this.rules = new List<JsonSchemaRule>();
            }

            public void Add(JsonSchemaRule rule)
            {
                this.rules.Add(rule);
            }

            public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
            {
                foreach (JsonSchemaRule rule in rules)
                    if (rule.IsValid(definitions, value) == false)
                        return false;

                return true;
            }
        }

        private void AddAdditionalPropertiesRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonFalse>("additionalProperties"))
            {
                JsonObject properties = definition.Get<JsonObject>("properties");
                JsonObject patterns = definition.Get<JsonObject>("patternProperties");
                JsonAdditionalPropertiesRule rule = new JsonAdditionalPropertiesRule();

                if (properties != null)
                    foreach (string property in properties.GetKeys())
                        rule.AddProperty(property);

                if (patterns != null)
                    foreach (string pattern in patterns.GetKeys())
                        rule.AddPattern(pattern);

                rules.Add(rule);
            }
        }

        private void AddAllOfRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonArray>("allOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("allOf");
                JsonAllOfRule rule = new JsonAllOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(Parse(item));

                rules.Add(rule);
            }
        }

        private void AddAnyOfRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonArray>("anyOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("anyOf");
                JsonAnyOfRule rule = new JsonAnyOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(Parse(item));

                rules.Add(rule);
            }
        }

        private void AddItemsRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonObject>("items"))
            {
                JsonObject items = definition.Get<JsonObject>("items");
                JsonItemsRule rule = new JsonItemsRule(Parse(items));

                rules.Add(rule);
            }
        }

        private void AddOneOfRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonArray>("oneOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("oneOf");
                JsonOneOfRule rule = new JsonOneOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(Parse(item));

                rules.Add(rule);
            }
        }

        private void AddPropertiesRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonObject>("properties"))
            {
                JsonObject properties = definition.Get<JsonObject>("properties");
                JsonPropertiesRule rule = new JsonPropertiesRule();

                foreach (string property in properties.GetKeys())
                    rule.Add(property, Parse(properties.Get<JsonObject>(property)));

                rules.Add(rule);
            }
        }

        private void AddRefRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonText>("$ref"))
            {
                string reference = definition.Get<JsonText>("$ref").Value;
                JsonRefRule rule = new JsonRefRule(reference);

                rules.Add(rule);
            }
        }

        private void AddRequiredRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonArray>("required"))
            {
                JsonArray required = definition.Get<JsonArray>("required");
                JsonRequiredRule rule = new JsonRequiredRule();

                foreach (JsonText property in required.Items())
                {
                    rule.Add(property.Value);
                }

                rules.Add(rule);
            }
        }

        private void AddTypeRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonText>("type"))
            {
                string type = definition.Get<JsonText>("type").Value;
                JsonTypeRule rule = new JsonTypeRule(type);

                rules.Add(rule);
            }
        }

        public JsonSchemaRule Parse(JsonObject definition)
        {
            CombinedRule combined = new CombinedRule();

            AddAdditionalPropertiesRule(combined, definition);
            AddAllOfRule(combined, definition);
            AddAnyOfRule(combined, definition);
            AddItemsRule(combined, definition);
            AddOneOfRule(combined, definition);
            AddPropertiesRule(combined, definition);
            AddRefRule(combined, definition);
            AddRequiredRule(combined, definition);
            AddTypeRule(combined, definition);

            return combined;
        }
    }
}