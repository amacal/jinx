using Jinx.Dom;
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
            JsonObject definition = value as JsonObject;

            if (definition == null)
                return null;

            JsonText type = definition.Get("type") as JsonText;

            switch (type?.Value)
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

            JsonAllRule allOfRule;
            JsonArray allOf = objekt.Get("allOf") as JsonArray;

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

            if (allOf != null)
            {
                allOfRule = new JsonAllRule(objectRule);
                result = allOfRule;

                foreach (JsonObject inner in allOf.Items().OfType<JsonObject>())
                {
                    allOfRule.Add(ParseObjectRule(inner));
                }
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
