using Jinx.Reader;
using System;

namespace Jinx.Dom
{
    public class JsonDocumentReader
    {
        private readonly JsonReader reader;

        public JsonDocumentReader(JsonReader reader)
        {
            this.reader = reader;
        }

        public JsonDocument Load()
        {
            JsonDocument document = null;

            Action<JsonValue> onRoot = root =>
            {
                document = new JsonDocument(root);
            };

            while (reader.Next())
            {
                ParseValue(onRoot);
            }

            return document;
        }

        private void ParseObject(Action<JsonObject> callback)
        {
            string name = null;
            JsonObject objekt = new JsonObject();

            Action<JsonValue> onValue = value =>
            {
                objekt.Add(name, value);
            };

            while (reader.Next())
            {
                switch (reader.Token.Type)
                {
                    case JsonTokenType.Property:
                        name = reader.Token.GetString();
                        if (reader.Next())
                        {
                            ParseValue(onValue);
                        }
                        break;

                    case JsonTokenType.EndObject:
                        callback.Invoke(objekt);
                        return;

                    default:
                        break;
                }
            }
        }

        private void ParseArray(Action<JsonArray> callback)
        {
            JsonArray array = new JsonArray();

            Action<JsonValue> onValue = value =>
            {
                array.Add(value);
            };

            while (reader.Next())
            {
                switch (reader.Token.Type)
                {
                    case JsonTokenType.EndArray:
                        callback.Invoke(array);
                        return;

                    default:
                        ParseValue(onValue);
                        break;
                }
            }
        }

        private void ParseValue(Action<JsonValue> callback)
        {
            switch (reader.Token.Type)
            {
                case JsonTokenType.OpenObject:
                    ParseObject(callback);
                    break;

                case JsonTokenType.OpenArray:
                    ParseArray(callback);
                    break;

                case JsonTokenType.Text:
                    callback.Invoke(new JsonText(reader.Token.GetString()));
                    break;

                case JsonTokenType.Number:
                    callback.Invoke(new JsonNumber(reader.Token.GetString()));
                    break;

                case JsonTokenType.True:
                    callback.Invoke(new JsonTrue());
                    break;

                case JsonTokenType.False:
                    callback.Invoke(new JsonFalse());
                    break;

                case JsonTokenType.Null:
                    callback.Invoke(new JsonNull());
                    break;

                default:
                    break;
            }
        }
    }
}