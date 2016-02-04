using System.Collections.Generic;

namespace Jinx.Dom
{
    public class JsonArray : JsonValue
    {
        private readonly List<JsonValue> items;

        public JsonArray()
        {
            this.items = new List<JsonValue>();
        }

        public void Add(JsonValue item)
        {
            items.Add(item);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public JsonValue Get(int index)
        {
            return items[index];
        }

        public IEnumerable<JsonValue> Items()
        {
            return items.AsReadOnly();
        }
    }
}