using Jinx.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Jinx.Schema
{
    public class JsonSchemaReader
    {
        private readonly JsonDocument document;

        public JsonSchemaReader(JsonDocument document)
        {
            this.document = document;
        }

        public JsonSchema Load()
        {
            return new JsonSchema(ParseSchema(document.Root));
        }

        private JsonSchemaRule ParseSchema(JsonValue value)
        {
            List<JsonSchemaRule> rules = new List<JsonSchemaRule>();
            JsonObject definition = value as JsonObject;

            if (definition == null)
                return null;

            JsonText singleType = definition.Get("type") as JsonText;
            JsonArray manyTypes = definition.Get("type") as JsonArray;

            JsonAllOfRule allOfRule;
            JsonArray allOf = definition.Get("allOf") as JsonArray;

            JsonAnyOfRule anyOfRule;
            JsonArray anyOf = definition.Get("anyOf") as JsonArray;

            JsonOneOfRule oneOfRule;
            JsonArray oneOf = definition.Get("oneOf") as JsonArray;

            if (singleType != null)
                rules.Add(ParseRule(definition, singleType.Value));

            if (manyTypes != null)
            {
                List<JsonSchemaRule> many = new List<JsonSchemaRule>();

                foreach (JsonText type in manyTypes.Items().OfType<JsonText>())
                {
                    many.Add(ParseRule(definition, type.Value));
                }

                if (many.Count > 0)
                {
                    rules.Add(new JsonAnyOfRule(many));
                }
            }

            if (allOf != null)
            {
                allOfRule = new JsonAllOfRule();
                rules.Add(allOfRule);

                foreach (JsonObject inner in allOf.Items().OfType<JsonObject>())
                {
                    allOfRule.Add(ParseSchema(inner));
                }
            }

            if (anyOf != null)
            {
                anyOfRule = new JsonAnyOfRule();
                rules.Add(anyOfRule);

                foreach (JsonObject inner in anyOf.Items().OfType<JsonObject>())
                {
                    anyOfRule.Add(ParseSchema(inner));
                }
            }

            if (oneOf != null)
            {
                oneOfRule = new JsonOneOfRule();
                rules.Add(oneOfRule);

                foreach (JsonObject inner in oneOf.Items().OfType<JsonObject>())
                {
                    oneOfRule.Add(ParseSchema(inner));
                }
            }

            if (rules.Count == 0)
                return new JsonNoRule();

            if (rules.Count == 1)
                return rules[0];

            return new JsonAllOfRule(rules);
        }

        private JsonSchemaRule ParseRule(JsonObject definition, string type)
        {
            switch (type)
            {
                case "object":
                    return ParseObjectRule(definition);

                case "array":
                    return ParseArrayRule(definition);

                case "string":
                    return ParseTextRule(definition);

                case "number":
                    return ParseNumberRule(definition);

                case "integer":
                    return ParseNumberRule(definition);
            }

            return new JsonNoRule();
        }

        private JsonSchemaRule ParseObjectRule(JsonObject objekt)
        {
            JsonSchemaRule result = null;
            JsonObjectRule objectRule = new JsonObjectRule();

            JsonObject properties = objekt.Get("properties") as JsonObject;
            JsonObject patterns = objekt.Get("patternProperties") as JsonObject;

            JsonArray required = objekt.Get("required") as JsonArray;
            JsonValue additional = objekt.Get("additionalProperties");

            if (properties != null)
            {
                foreach (string property in properties.GetKeys())
                {
                    objectRule.AddProperty(property, ParseSchema(properties.Get(property)));
                }
            }

            if (patterns != null)
            {
                foreach (string property in patterns.GetKeys())
                {
                    objectRule.AddPattern(property, ParseSchema(patterns.Get(property)));
                }
            }

            if (required != null)
            {
                foreach (JsonText text in required.Items().OfType<JsonText>())
                {
                    objectRule.AddRequired(text.Value);
                }
            }

            if (additional is JsonTrue)
            {
                objectRule.SetAdditional(true);
            }

            if (additional is JsonFalse)
            {
                objectRule.SetAdditional(false);
            }

            return result ?? objectRule;
        }

        private JsonArrayRule ParseArrayRule(JsonObject objekt)
        {
            JsonArrayRule rule = new JsonArrayRule();
            JsonObject items = objekt.Get("items") as JsonObject;

            if (items != null)
            {
                rule.SetSchema(ParseSchema(items));
            }

            return rule;
        }

        private JsonTextRule ParseTextRule(JsonObject objekt)
        {
            JsonTextRule rule = new JsonTextRule();

            return rule;
        }

        private JsonNumberRule ParseNumberRule(JsonObject objekt)
        {
            JsonNumberRule rule = new JsonNumberRule();

            return rule;
        }

        private JsonIntegerRule ParseIntegerRule(JsonObject objekt)
        {
            JsonIntegerRule rule = new JsonIntegerRule();

            return rule;
        }
    }
}