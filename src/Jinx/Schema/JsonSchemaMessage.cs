using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonSchemaMessage
    {
        private readonly JsonSchemaPath path;
        private readonly JsonValue value;
        private readonly string description;

        public JsonSchemaMessage(JsonSchemaPath path, JsonValue value, string description)
        {
            this.path = path;
            this.value = value;
            this.description = description;
        }

        public JsonSchemaPath Path
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