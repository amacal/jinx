using System.Text;

namespace Jinx.Path.Segments
{
    public class JsonPropertySegment : JsonPathSegment
    {
        private readonly string property;

        public JsonPropertySegment(string property)
        {
            this.property = property;
        }

        public override void Write(StringBuilder builder, int index)
        {
            if (index > 0)
                builder.Append('.');

            builder.Append(property);
        }
    }
}