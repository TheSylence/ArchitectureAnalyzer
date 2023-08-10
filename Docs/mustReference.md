# MustReference

| Category                | Required                        | Forbidden                           |
|-------------------------|---------------------------------|-------------------------------------|
| **Rule Id**             | AA0007                          | AA0008                              |
| **Summary**             | Type must reference other type. | Type must not reference other type. |
| **Supported Languages** | CSharp                          | CSharp                              |
| **Tags**                | Design                          | Design                              |
| **Severity**            | Warning                         | Warning                             |

## Schema
    
```json
{
    "mustReference": {
        "forbidden": "<boolean>",
        "forTypes": "<matcher>",
        "reference": "<matcher>"
    }
}
```

**reference**: The type that must be referenced.

## Examples

```json
{
  "mustReference": {
    "reference": { "namespace": "*.Service" }    ,
    "forTypes": { "namespace": "*.Tests" }
  }
}
```

(Every type in a namespace that ends with "Tests" must reference a type in a namespace that ends with "Service".)

```json
{
  "mustReference": {
    "forbidden": true,
    "reference": { "namespace": "*.Tests" },
    "forTypes": { "namespace": "*.Services" }
  }
}
```

(Every type in a namespace that ends with "Services" must not reference a type in a namespace that ends with "Tests".)