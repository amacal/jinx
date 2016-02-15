using System.Text;

namespace Jinx.Path
{
    public abstract class JsonPathSegment
    {
        public abstract void Write(StringBuilder builder, int index);
    }
}