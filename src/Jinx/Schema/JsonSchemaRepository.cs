using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Jinx.Schema
{
    public class JsonSchemaRepository
    {
        private readonly Dictionary<string, JsonSchema> schemas;

        public JsonSchemaRepository()
        {
            this.schemas = new Dictionary<string, JsonSchema>();
        }

        public JsonSchema GetByReference(string reference)
        {
            if (schemas.ContainsKey(reference) == false)
            {
                using (WebClient client = new WebClient())
                using (Stream stream = client.OpenRead(reference))
                {
                    schemas.Add(reference, JsonConvert.GetSchema(stream));
                }
            }

            return schemas[reference];
        }
    }
}
