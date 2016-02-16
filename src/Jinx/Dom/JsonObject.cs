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

        public int Count
        {
            get { return items.Count; }
        }

        public bool Contains(string name)
        {
            return items.ContainsKey(name);
        }

        public bool Contains<T>(string name)
            where T : JsonValue
        {
            return items.ContainsKey(name) && items[name] is T;
        }

        public JsonValue Get(string name)
        {
            JsonValue value;
            items.TryGetValue(name, out value);
            return value;
        }

        public T Get<T>(string name)
            where T : JsonValue
        {
            return Get(name) as T;
        }

        public IEnumerable<string> GetKeys()
        {
            return items.Keys;
        }

        public override string ToString()
        {
            return "object";
        }
    }
}