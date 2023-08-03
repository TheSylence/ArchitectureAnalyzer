# ArchitectureAnalyzer

![GitHub](https://img.shields.io/github/license/TheSylence/ArchitectureAnalyzer)
![Nuget](https://img.shields.io/nuget/v/ArchitectureAnalyzer)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/TheSylence/ArchitectureAnalyzer/CI.yml)

Roslyn analyzers that allow you to define architectural rules for your codebase

# Installation
### 1. Install package
Install the [NuGet package](https://www.nuget.org/packages/ArchitectureAnalyzer/) in your project:

```powershell
Install-Package ArchitectureAnalyzer
```

or

```powershell
dotnet add package ArchitectureAnalyzer
```

### 2. Configure project
Add a file called "architecture.rules.json" as an AdditionalFile to your project. This file will contain the rules for your project.
```xml
<ItemGroup>
    <AdditionalFiles Include="architecture.rules.json" />
</ItemGroup>
```

### 3. Define rules
Define the rules for your project in the "architecture.rules.json" file.
The schema for this file is:

```json
{
    "rules": [
      "<rule>",
      "<rule>",
      "..."
    ]
}
```

# Rules

[See rules documentation](Docs/rules.md)

# License
MIT License ([Read license](LICENSE))

Uses [LightJson](https://github.com/MarcosLopezC/LightJson) by Marcos López C.