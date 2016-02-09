namespace Jinx.Dom
{
    public class JsonDocument
    {
        private readonly JsonValue root;

        public JsonDocument(JsonValue root)
        {
            this.root = root;
        }

        public JsonValue Root
        {
            get { return root; }
        }
    }
}