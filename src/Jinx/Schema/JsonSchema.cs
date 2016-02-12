using Jinx.Dom;
using Jinx.Reader;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Jinx.Schema
{
    public class JsonSchema
    {
        public static readonly JsonSchema Draft04;

        private readonly JsonSchemaRule rule;
        private readonly JsonSchemaDefinitions definitions;

        static JsonSchema()
        {
            Draft04 = GetSchema("draft-04");
        }

        public JsonSchema(JsonSchemaRule rule, JsonSchemaDefinitions definitions)
        {
            this.rule = rule;
            this.definitions = definitions;
        }

        public bool IsValid(JsonValue value)
        {
            return rule.IsValid(definitions, value, JsonSchemaCallback.Ignore());
        }

        public bool IsValid(JsonValue value, ICollection<JsonSchemaMessage> messages)
        {
            return rule.IsValid(definitions, value, new JsonSchemaCallback(messages));
        }

        private static JsonSchema GetSchema(string name)
        {
            Assembly assembly = typeof(JsonSchema).Assembly;
            string fullName = $"Resources.{name}";

            using (Stream stream = assembly.GetManifestResourceStream(typeof(JsonSchema), fullName))
            using (TextReader reader = new StreamReader(stream))
            {
                var jsonReader = new JsonReader(reader);
                var documentReader = new JsonDocumentReader(jsonReader);
                var document = documentReader.Load();
                var schemaReader = new JsonSchemaReader(document);

                return schemaReader.Load();
            }
        }
    }
}