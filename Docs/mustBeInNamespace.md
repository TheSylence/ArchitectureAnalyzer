# MustBeInNamespace

| Category                | Required                               | Forbidden                                  |
|-------------------------|----------------------------------------|--------------------------------------------|
| **Rule Id**             | AA0009                                 | AA0010                                     |
| **Summary**             | Type must be in a specified namespace. | Type must not be in a specified namespace. |
| **Supported Languages** | CSharp                                 | CSharp                                     |
| **Tags**                | Design                                 | Design                                     |
| **Severity**            | Warning                                | Warning                                    |

## Schema

```json
{
    "mustBeInNamespace": {
        "forbidden": "<boolean>",
        "forTypes": "<matcher>",
        "namespace": "<string>"
    }
}
```

**namespace**: The namespace that the type must (not) be in. (Wildcards (`*`) are supported.)

## Examples

```json
{
  "mustBeInNamespace": {
    "namespace": "MyNamespace.*",
    "forTypes": { "name": "*" }
  }
}
```

(Every type must be in the namespace "MyNamespace" or a sub-namespace thereof.)
