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

            Action<JsonObject> onObject = objekt =>
            {
                document = new JsonDocument(objekt);
            };

            Action<JsonArray> onArray = array =>
            {
                document = new JsonDocument(array);
            };

            while (reader.Next())
            {
                switch (reader.Token.Type)
                {
                    case JsonTokenType.OpenObject:
                        ParseObject(onObject);
                        break;

                    case JsonTokenType.OpenArray:
                        ParseArray(onArray);
                        break;
                }
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
                    callback.Invoke(new JsonNumber());
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
            }
        }
    }
}