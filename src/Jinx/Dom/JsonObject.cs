using System.Collections.Generic;

namespace Jinx.Dom
{
    public class JsonObject : JsonValue
    {
        private readonly Dictionary<string, JsonValue> items;

        public JsonObject()
        {
            this.items = new Dictionary<string, JsonValue>();
        }

        public void Add(string name, JsonValue value)
        {
            items[name] = value;
        }

        public bool Contains(string name)
        {
            return items.ContainsKey(name);
        }

        public JsonValue Get(string name)
        {
            JsonValue value;
            items.TryGetValue(name, out value);
            return value;
        }

        public IEnumerable<string> GetKeys()
        {
            return items.Keys;
        }
    }
}