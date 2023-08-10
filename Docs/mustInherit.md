# MustInherit

| Category                | Required                          | Forbidden                             |
|-------------------------|-----------------------------------|---------------------------------------|
| **Rule Id**             | AA0002                            | AA0004                                |
| **Summary**             | Type must inherit specified type. | Type must not inherit specified type. |
| **Supported Languages** | CSharp                            | CSharp                                |
| **Tags**                | Design                            | Design                                |
| **Severity**            | Warning                           | Warning                               |

## Schema
    
```json
{
    "mustInherit": {
        "forbidden": "<boolean>",
        "forTypes": "<matcher>",
        "baseType": "<matcher>"
    }
}
```

**baseType**: The base type that must be inherited.

## Examples

```json
{
  "mustInherit": {
    "baseType": { "fullName": "System.Exception" },
    "forTypes": { "name": "*Exception" }
  }
}
```

(Every type that ends with "Exception" must inherit System.Exception.)

```json
{
  "mustInherit": {
    "forbidden": true,
    "baseType": { "fullName": "System.Exception" } ,
    "forTypes": { "not": {"name": "*Exception" } }
  }
}
```

(Every type that does not end with "Exception" must not inherit from System.Exception.)