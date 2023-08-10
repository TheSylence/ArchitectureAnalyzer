# RelatedTypeExists


| Category                | Required                                             | Forbidden                                                |
|-------------------------|------------------------------------------------------|----------------------------------------------------------|
| **Rule Id**             | AA0005                                               | AA0006                                                   |
| **Summary**             | Type that is related to the matched type must exist. | Type that is related to the matched type must not exist. |
| **Supported Languages** | CSharp                                               | CSharp                                                   |
| **Tags**                | Design                                               | Design                                                   |
| **Severity**            | Warning                                              | Warning                                                  |

## Schema
    
```json
{
    "relatedTypeExists": {
        "forbidden": "<boolean>",
      "forTypes": "<matcher>",
        "relatedType": "<matcher>"
    }
}
```

**relatedType**: The type that must exist.
Use `%type.PROPERTY%` to reference the matched type in a name-based matcher where `PROPERTY` is one of the following.

| Property  | Description                  |
|-----------|------------------------------|
| Name      | The name of the type.        |
| Namespace | The namespace of the type.   |
| FullName  | The full name of the type.   |

## Examples

```json
{
    "relatedTypeExists": {
        "relatedType": {
            "name": "%type.Name%Repository"
        },
        "forTypes": {
            "name": "*Service"
        }
    }
}
```

All types ending with *Service* must have a related type ending with *Repository*.

```json
{
    "relatedTypeExists": {
        "relatedType": {
            "implements": { "generic": {"type": {"Name": "IValidator"}, "typeArguments": [{ "fullName": "%type.FullName%"}] } }
        },
        "forTypes": {
            "name": "*Request"
        }
    }
}
```

All types ending with *Request* must have a related type that implements *IValidator&lt;T&gt;* with the type of the matched type as type argument.

