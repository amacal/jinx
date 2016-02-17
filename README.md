### jinx

* parses json into document object model
* validates json against json-schema (draft-04)

##### deserialize stream into document and validate it against given schema

``` csharp
  JsonDocument document = JsonConvert.GetDocument(streamWithData);
  JsonSchema schema = JsonConvert.GetSchema(pathToSchema);
  
  List<JsonSchemaMessage> violations = new List<JsonSchemaMessage>();
  bool succeeded = schema.IsValid(document, violations);
  
  if (succeeded == false)
  {
    foreach (JsonSchemaMessage violation in violations)
    {
      Console.WriteLine($"{violation.Path}: {violation.Message}");
    }
  }
```
