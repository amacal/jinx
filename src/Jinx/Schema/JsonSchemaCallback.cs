using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonSchemaCallback
    {
        public static JsonSchemaCallback Ignore()
        {
            return new JsonSchemaCallback();
        }

        private readonly ICollection<JsonSchemaMessage> items;
        private readonly JsonSchemaPath path;

        private JsonSchemaCallback()
        {
            this.path = JsonSchemaPath.Root;
        }

        private JsonSchemaCallback(JsonSchemaPath path)
        {
            this.path = path;
        }

        public JsonSchemaCallback(ICollection<JsonSchemaMessage> items)
        {
            this.items = items;
            this.path = JsonSchemaPath.Root;
        }

        public JsonSchemaCallback Drill(string name)
        {
            return new JsonSchemaCallback(path.Drill(name));
        }

        public bool Call(JsonValue value, string description)
        {
            if (items != null)
            {
                items.Add(new JsonSchemaMessage(path, value, description));
            }

            return false;
        }

        public bool Call(string name, JsonValue value, string description)
        {
            if (items != null)
            {
                items.Add(new JsonSchemaMessage(path.Drill(name), value, description));
            }

            return false;
        }

        public void Add(JsonSchemaMessage message)
        {
            if (items != null)
            {
                items.Add(message);
            }
        }
    }
}