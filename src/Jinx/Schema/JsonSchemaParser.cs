using Jinx.Dom;
using Jinx.Schema.Factories;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonSchemaParser
    {
        private static readonly List<JsonSchemaRuleFactory> factories;

        static JsonSchemaParser()
        {
            factories = new List<JsonSchemaRuleFactory>
            {
                new JsonAdditionalItemsRuleFactory(),
                new JsonAdditionalPropertiesRuleFactory(),
                new JsonAllOfRuleFactory(),
                new JsonAnyOfRuleFactory(),
                new JsonDependencyRuleFactory(),
                new JsonEnumRuleFactory(),
                new JsonFormatRuleFactory(),
                new JsonItemsRuleFactory(),
                new JsonMaximumRuleFactory(),
                new JsonMaxItemsRuleFactory(),
                new JsonMaxLengthRuleFactory(),
                new JsonMaxPropertiesRuleFactory(),
                new JsonMinimumRuleFactory(),
                new JsonMinItemsRuleFactory(),
                new JsonMinLengthRuleFactory(),
                new JsonMinPropertiesRuleFactory(),
                new JsonMultipleOfRuleFactory(),
                new JsonNotRuleFactory(),
                new JsonOneOfRuleFactory(),
                new JsonPatternPropertiesRuleFactory(),
                new JsonPatternRuleFactory(),
                new JsonPropertiesRuleFactory(),
                new JsonRefRuleFactory(),
                new JsonRequiredRuleFactory(),
                new JsonTypeRuleFactory(),
                new JsonUniqueItemsRuleFactory()
            };
        }

        public JsonSchemaRule Parse(JsonObject definition)
        {
            JsonSchemaRuleComponent component = new JsonSchemaRuleComponent();

            foreach (JsonSchemaRuleFactory factory in factories)
                factory.Append(component, definition, Parse);

            return component;
        }
    }
}