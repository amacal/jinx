using Jinx.Dom;
using Jinx.Schema.Rules;
using System;
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

            public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
            {
                foreach (JsonSchemaRule rule in rules)
                    if (rule.IsValid(definitions, value, callback) == false)
                        return false;

                return true;
            }
        }

        private void AddAdditionalItemsRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonFalse>("additionalItems"))
            {
                if (definition.Contains<JsonArray>("items"))
                {
                    JsonArray items = definition.Get<JsonArray>("items");
                    JsonAdditionalItemsRule rule = new JsonAdditionalItemsRule(items.Count);

                    rules.Add(rule);
                }
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

        private void AddEnumRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonArray>("enum"))
            {
                JsonArray enums = definition.Get<JsonArray>("enum");
                JsonEnumRule rule = new JsonEnumRule();

                foreach (JsonValue value in enums.Items())
                    rule.Add(value);

                rules.Add(rule);
            }
        }

        private void AddFormatRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonText>("format"))
            {
                JsonText format = definition.Get<JsonText>("format");
                JsonFormatRule rule = new JsonFormatRule(format.Value);

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

        private void AddMaximumRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("maximum"))
            {
                JsonNumber maximum = definition.Get<JsonNumber>("maximum");
                JsonMaximumRule rule = new JsonMaximumRule(Decimal.Parse(maximum.Value));

                rules.Add(rule);
            }
        }

        private void AddMaxItemsRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("maxItems"))
            {
                JsonNumber maxItems = definition.Get<JsonNumber>("maxItems");
                JsonMaxItemsRule rule = new JsonMaxItemsRule(Int32.Parse(maxItems.Value));

                rules.Add(rule);
            }
        }

        private void AddMaxLengthRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("maxLength"))
            {
                JsonNumber maxLength = definition.Get<JsonNumber>("maxLength");
                JsonMaxLengthRule rule = new JsonMaxLengthRule(Int32.Parse(maxLength.Value));

                rules.Add(rule);
            }
        }

        private void AddMaxPropertiesRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("maxProperties"))
            {
                JsonNumber maxProperties = definition.Get<JsonNumber>("maxProperties");
                JsonMaxPropertiesRule rule = new JsonMaxPropertiesRule(Int32.Parse(maxProperties.Value));

                rules.Add(rule);
            }
        }

        private void AddMinimumRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("minimum"))
            {
                JsonNumber minimum = definition.Get<JsonNumber>("minimum");
                JsonMinimumRule rule = new JsonMinimumRule(Decimal.Parse(minimum.Value));

                rules.Add(rule);
            }
        }

        private void AddMinItemsRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("minItems"))
            {
                JsonNumber minItems = definition.Get<JsonNumber>("minItems");
                JsonMinItemsRule rule = new JsonMinItemsRule(Int32.Parse(minItems.Value));

                rules.Add(rule);
            }
        }

        private void AddMinLengthRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("minLength"))
            {
                JsonNumber minLength = definition.Get<JsonNumber>("minLength");
                JsonMinLengthRule rule = new JsonMinLengthRule(Int32.Parse(minLength.Value));

                rules.Add(rule);
            }
        }

        private void AddMinPropertiesRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonNumber>("minProperties"))
            {
                JsonNumber minProperties = definition.Get<JsonNumber>("minProperties");
                JsonMinPropertiesRule rule = new JsonMinPropertiesRule(Int32.Parse(minProperties.Value));

                rules.Add(rule);
            }
        }

        private void AddNotRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonObject>("not"))
            {
                JsonObject not = definition.Get<JsonObject>("not");
                JsonNotRule rule = new JsonNotRule(Parse(not));

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

        private void AddPatternPropertiesRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonObject>("patternProperties"))
            {
                JsonObject patterns = definition.Get<JsonObject>("patternProperties");
                JsonPatternPropertiesRule rule = new JsonPatternPropertiesRule();

                foreach (string pattern in patterns.GetKeys())
                    rule.AddPattern(pattern, Parse(patterns.Get<JsonObject>(pattern)));

                rules.Add(rule);
            }
        }

        private void AddPatternRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonText>("pattern"))
            {
                JsonText pattern = definition.Get<JsonText>("pattern");
                JsonPatternRule rule = new JsonPatternRule(pattern.Value);

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
                    rule.AddProperty(property, Parse(properties.Get<JsonObject>(property)));

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

        private void AddUniqueItemsRule(CombinedRule rules, JsonObject definition)
        {
            if (definition.Contains<JsonTrue>("uniqueItems"))
            {
                rules.Add(new JsonUniqueItemsRule());
            }
        }

        public JsonSchemaRule Parse(JsonObject definition)
        {
            CombinedRule combined = new CombinedRule();

            AddAdditionalItemsRule(combined, definition);
            AddAdditionalPropertiesRule(combined, definition);
            AddAllOfRule(combined, definition);
            AddAnyOfRule(combined, definition);
            AddEnumRule(combined, definition);
            AddFormatRule(combined, definition);
            AddItemsRule(combined, definition);
            AddMaximumRule(combined, definition);
            AddMaxItemsRule(combined, definition);
            AddMaxLengthRule(combined, definition);
            AddMaxPropertiesRule(combined, definition);
            AddMinimumRule(combined, definition);
            AddMinItemsRule(combined, definition);
            AddMinLengthRule(combined, definition);
            AddMinPropertiesRule(combined, definition);
            AddNotRule(combined, definition);
            AddOneOfRule(combined, definition);
            AddPatternPropertiesRule(combined, definition);
            AddPatternRule(combined, definition);
            AddPropertiesRule(combined, definition);
            AddRefRule(combined, definition);
            AddRequiredRule(combined, definition);
            AddTypeRule(combined, definition);
            AddUniqueItemsRule(combined, definition);

            return combined;
        }
    }
}