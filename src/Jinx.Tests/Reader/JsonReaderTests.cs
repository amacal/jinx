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
        [InlineData(@"truk")]
        [InlineData(@"falsi")]
        [InlineData(@"nuul")]
        [InlineData(@"[,")]
        [InlineData(@"""\k""")]
        [InlineData(@"""\u004Z""")]
        public void CanHandleSyntaxError(string data)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next()) { }

                Assert.True(reader.HasError);
            }
        }

        [Theory]
        [InlineData(@"  ")]
        [InlineData(@"[")]
        [InlineData(@"[true")]
        [InlineData(@"[false,")]
        [InlineData(@"{")]
        [InlineData(@"{""")]
        [InlineData(@"{""abc")]
        [InlineData(@"{""abc""")]
        [InlineData(@"{""abc"":true")]
        [InlineData(@"{""abc"":false,")]
        [InlineData(@"""")]
        [InlineData(@"""abc")]
        [InlineData(@"""\")]
        [InlineData(@"""\u")]
        [InlineData(@"""\u12")]
        [InlineData(@"nul")]
        [InlineData(@"tru")]
        [InlineData(@"fal")]
        public void CanHandleStreamError(string data)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next()) { }

                Assert.True(reader.HasError);
            }
        }
    }
}