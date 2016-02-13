using Jinx.Dom;
using Jinx.Schema.Rules;
using System;
using System.Collections.Generic;

namespace Jinx.Schema.Factories
{
    public class JsonDependencyRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonObject>("dependencies"))
            {
                JsonObject dependencies = definition.Get<JsonObject>("dependencies");
                JsonDependencyRule rule = new JsonDependencyRule();

                foreach (string property in dependencies.GetKeys())
                {
                    if (dependencies.Contains<JsonArray>(property))
                    {
                        JsonArray items = dependencies.Get<JsonArray>(property);
                        List<string> values = new List<string>(items.Count);

                        foreach (JsonText item in items.Items<JsonText>())
                            values.Add(item.Value);

                        rule.Add(property, values.ToArray());
                    }

                    if (dependencies.Contains<JsonObject>(property))
                    {
                        JsonObject container = dependencies.Get<JsonObject>(property);
                        JsonSchemaRule schema = parse(container);

                        rule.Add(property, schema);
                    }
                }

                rules.Add(rule);
            }
        }
    }
}