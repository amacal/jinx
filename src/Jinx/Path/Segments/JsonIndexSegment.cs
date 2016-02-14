using System.Text;

namespace Jinx.Path.Segments
{
    public class JsonIndexSegment : JsonPathSegment
    {
        private readonly int index;

        public JsonIndexSegment(int index)
        {
            this.index = index;
        }

        public override void Write(StringBuilder builder, int index)
        {
            builder.Append('[');
            builder.Append(index);
            builder.Append(']');
        }
    }
}
