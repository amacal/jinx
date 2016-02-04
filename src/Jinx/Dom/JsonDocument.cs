namespace Jinx.Dom
{
    public class JsonDocument
    {
        private readonly JsonValue root;

        public JsonDocument(JsonObject root)
        {
            this.root = root;
        }

        public JsonDocument(JsonArray root)
        {
            this.root = root;
        }

        public JsonValue Root
        {
            get { return root; }
        }
    }
}