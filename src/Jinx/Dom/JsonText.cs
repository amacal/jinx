namespace Jinx.Dom
{
    public class JsonText : JsonValue
    {
        private readonly string value;

        public JsonText(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}