namespace Jinx.Reader
{
    public abstract class JsonToken
    {
        public abstract JsonTokenType Type { get; }

        public abstract string GetString();
    }
}