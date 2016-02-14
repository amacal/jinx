using System.Collections.Generic;
using System.Text;

namespace Jinx.Path
{
    public class JsonPath
    {
        public static readonly JsonPath Root;

        static JsonPath()
        {
            Root = new JsonPath();
        }

        private readonly IList<JsonPathSegment> items;

        private JsonPath()
        {
            this.items = new JsonPathSegment[0];
        }

        private JsonPath(JsonPath path, JsonPathSegment segment)
        {
            this.items = new List<JsonPathSegment>(path.items);
            this.items.Add(segment);
        }

        public JsonPath Append(JsonPathSegment segment)
        {
            return new JsonPath(this, segment);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < items.Count; i++)
                items[i].Write(builder, i);

            return builder.ToString();
        }
    }
}