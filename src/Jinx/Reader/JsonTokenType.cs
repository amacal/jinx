namespace Jinx.Reader
{
    public enum JsonTokenType
    {
        OpenObject,
        EndObject,
        Property,

        OpenArray,
        EndArray,

        Text,
        Number,
        True,
        False,
        Null
    }
}