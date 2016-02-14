using Jinx.Dom;
using Jinx.Path;

namespace Jinx.Schema
{
    public class JsonSchemaMessage
    {
        private readonly JsonPath path;
        private readonly JsonValue value;
        private readonly string description;

        public JsonSchemaMessage(JsonPath path, JsonValue value, string description)
        {
            this.path = path;
            this.value = value;
            this.description = description;
        }

        public JsonPath Path
        {
            get { return path; }
        }

        public JsonValue Value
        {
            get { return value; }
        }

        public string Description
        {
            get { return description; }
        }
    }
}