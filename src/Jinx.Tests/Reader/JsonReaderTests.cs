using Jinx.Reader;
using System;
using System.Globalization;
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
        [InlineData(@"""\u004f""", "O")]
        [InlineData(@"""\\""", "\\")]
        [InlineData(@"""\/""", "/")]
        [InlineData(@"""\""""", "\"")]
        public void CanReadTextValue(string data, string value)
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
        [InlineData(@"1", 1)]
        [InlineData(@"1.0", 1)]
        [InlineData(@"1.5", 1.5)]
        [InlineData(@"0.15", 0.15)]
        [InlineData(@"-1", -1)]
        [InlineData(@"1e2", 100)]
        [InlineData(@"1.1e+3", 1100)]
        [InlineData(@"1.2e-3", 0.0012)]
        public void CanReadNumberValue(string data, decimal value)
        {
            using (TextReader stream = new StringReader(data))
            {
                JsonReader reader = new JsonReader(stream);

                while (reader.Next())
                {
                    Assert.Equal(value, Decimal.Parse(reader.Token.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture));
                }
            }
        }

        [Theory]
        [InlineData(@"truk")]
        [InlineData(@"falsi")]
        [InlineData(@"nuul")]
        [InlineData(@"dim")]
        [InlineData(@"[,")]
        [InlineData(@"[true,]")]
        [InlineData(@"[true1]")]
        [InlineData(@"{,")]
        [InlineData(@"{true,")]
        [InlineData(@"{""a"",")]
        [InlineData(@"{""a"":true,]")]
        [InlineData(@"{""a"":true1]")]
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