namespace Jinx.Reader
{
    public enum JsonReaderState
    {
        Start,
        Final,

        SyntaxError,
        StreamError,

        BeginObject,
        BeginArray,

        Property,
        ValueInObject,
        ValueInArray,
    }
}