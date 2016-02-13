using Jinx.Reader;
using System.IO;
using Xunit;

namespace Jinx.Tests.Reader
{
    public class JsonReaderTests
    {
        [Theory]
        [InlineData(@"{}")]
        [InlineData(@"[]")]
        [InlineData(@"""abc""")]
        [InlineData(@"""\r\n""")]
        [InlineData(@"""\u0041""")]
        public void CanReadData(string data)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next())
                {
                    Assert.False(reader.HasError);
                }

                Assert.False(reader.HasError);
            }
        }

        [Theory]
        [InlineData(@"""abc""", "abc")]
        [InlineData(@"""\r\n\b\f\t""", "\r\n\b\f\t")]
        [InlineData(@"""\u0041""", "A")]
        [InlineData(@"""\\""", "\\")]
        [InlineData(@"""\/""", "/")]
        [InlineData(@"""\""""", "\"")]
        public void CanReadValue(string data, string value)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next())
                {
                    Assert.Equal(value, reader.Token.GetString());
                }
            }
        }

        [Theory]
        [InlineData(@"""\k""")]
        [InlineData(@"""\u004Z""")]
        public void CanHandleSyntaxError(string data)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next())
                {
                }

                Assert.True(reader.HasError);
            }
        }
    }
}
