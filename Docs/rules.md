﻿# Matchers

Matchers are used to match a set of types. 
They are used in every rule to determine if the rule should be applied to a given type via the `forTypes` property.
Some rules may also use them for their logic.

## Name

Matches types by their name.
The name of a type is the name of the type itself, not including the namespace.

### Syntax
```json
{ "name": "<expression>" }
```

Wildcards (`*`) are supported.

### Examples

```json
{ "name": "*" }
```
Matches all types.

```json
{ "name": "*MyType" }
```
Matches all types ending with *MyType*.

```json
{ "name": "MyType*" }
```
Matches all types starting with *MyType*.

```json
{ "name": "*MyType*" }
```
Matches all types containing *MyType*.

## FullName

Matches types by their full name.
The full name of a type is the name of the type including the namespace.

### Syntax
```json
{ "fullName": "<expression>" }
```

Wildcards (`*`) are supported.

### Examples

```json
{ "fullName": "MyNamespace.MyType" }
```
Matches the Type *MyType* in the namespace *MyNamespace*.

```json
{ "fullName": "MyNamespace.*" }
```
Matches all types in the namespace *MyNamespace*.

```json
{ "fullName": "*.MyType" }
```
Matches all types named *MyType* in any namespace.

```json
{ "fullName": "*" }
```
Matches all types.

## HasAttribute

Matches types that have a specific attribute.

### Syntax
```json
{ "hasAttribute": "<matcher>" }
```

### Examples

```json
{ "hasAttribute": { "name": "MyAttribute" } }
```
Matches all types that have the attribute *MyAttribute*.

## Inherits

Matches types that inherit from a specific type.

### Syntax
```json
{ "inherits": "<matcher>" }
```

### Examples

```json
{ "inherits": { "name": "MyNamespace.MyBaseType" } }
```
Matches all types that inherit from *MyBaseType* in the namespace *MyNamespace*.

```json
{ "inherits": { "name": "MyBaseType" } }
```
Matches all types that inherit from *MyBaseType* in any namespace.

```json
{ "inherits": { "name": "*" } }
```
Matches all types that inherit from any type.

## Implements

Matches types that implement a specific interface.

### Syntax
```json
{ "implements": "<matcher>" }
```

### Examples

```json
{ "implements": { "name": "MyNamespace.IMyInterface" } }
```
Matches all types that implement *IMyInterface* in the namespace *MyNamespace*.

```json
{ "implements": { "name": "IMyInterface" } }
```
Matches all types that implement *IMyInterface* in any namespace.

```json
{ "implements": { "name": "*" } }
```
Matches all types that implement any interface.

## And

Matches types that match all of the given matchers.

### Syntax
```json
{ "and": [ "<matcher>", "<matcher>", ... ] }
```

### Examples

```json
{ "and": [ { "name": "MyNamespace.MyType" }, { "hasAttribute": "MyAttribute" } ] }
```
Matches all types that are named *MyType* in the namespace *MyNamespace* and have the attribute *MyAttribute*.

## Or

Matches types that match any of the given matchers.

### Syntax
```json
{ "or": [ "<matcher>", "<matcher>", ... ] }
```

### Examples

```json
{ "or": [ { "name": "MyNamespace.MyType" }, { "name": "MyNamespace.MyOtherType" } ] }
```
Matches all types that are named *MyType* or *MyOtherType* in the namespace *MyNamespace*.

## Not

Matches types that do not match the given matcher.

### Syntax
```json
{ "not": "<matcher>" }
```

### Examples

```json
{ "not": { "name": "MyType" } }
```
Matches all types that are not named *MyType*.