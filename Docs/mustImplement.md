# MustImplement

| Category                | Required                                   | Forbidden                                      |
|-------------------------|--------------------------------------------|------------------------------------------------|
| **Rule Id**             | AA0001                                     | AA0003                                         |
| **Summary**             | Type must implement a specified interface. | Type must not implement a specified interface. |
| **Supported Languages** | CSharp                                     | CSharp                                         |
| **Tags**                | Design                                     | Design                                         |
| **Severity**            | Warning                                    | Warning                                        |

## Schema
    
```json
{
    "mustImplement": {
        "forbidden": "<boolean>",
        "forTypes": "<matcher>",
        "interface": "<matcher>"
    }
}
```

**interface**: The interface that must be implemented.

## Examples

```json
{
  "mustImplement": {
    "interface": { "fullName": "System.IDisposable" },
    "forTypes": { "name": "*Disposable" }
  }
}
```

(Every type that ends with "Disposable" must implement IDisposable.)

```json
{
  "mustImplement": {
    "forbidden": true,
    "interface": { "fullName": "System.IDisposable" }    ,
    "forTypes": { "name": "*UnDisposable" }
  }
}
```

(Every type that ends with "UnDisposable" must not implement IDisposable.)