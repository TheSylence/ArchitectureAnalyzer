# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.6.1] - 2023-09-01
### Fixed
- Nested types are now correctly recognized

## [0.6] - 2023-08-30
### Fixed
- Messages containing GenericMatcher are now correctly formatted
- Messages containing related types now use replaced values

## [0.5] - 2023-08-23
### Added
- MustBeInNamespaceRule to check whether types are contained in a specific namespace

### Fixed
- Distributed nuget package not referenced as analyzer

## [0.4] - 2023-08-14
### Added
- Namespace matcher to check if type is contained in specific namespace
- MustReference rule to enforce type must (not) reference other type

## [0.3] - 2023-08-08
### Added
- Forbidden Property to rules to replace must-not-x rules
- Description property to document rules
- IsMatcher for matching types based on kind, accessibility or modifiers
- RelatedTypeExists rule to check for related types

### Removed
- MustNotImplement and MustNotInherit. Replaced by forbidden property on rule

### Fixed
- Inherits matcher not matching for inherited base type
- IsMatcher now has human readable string representation
- implements and inherits matcher now uses the schema that is documented

## [0.2] - 2023-08-04
### Added
- Matcher for generic types

### Fixed
- Implements matcher not loaded from rules
- Inherits matcher not loaded from rules

## [0.1] - 2023-08-04
### Added
- MustImplement rule
- MustInherit rule
- MustNotImplement rule
- MustNotInherit rule


[0.6.1]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.6...0.6.1
[0.6]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.5...0.6
[0.5]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.4...0.5
[0.4]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.3...0.4
[0.3]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.2...0.3
[0.2]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.1...0.2
[0.1]: https://github.com/TheSylence/ArchitectureAnalyzer/releases/tag/0.1
<!-- Release: %URL%/releases/tag/%VERSION% -->
<!-- Compare: %URL%/compare/%OLD_VERSION%...%NEW_VERSION% -->
<!-- BaseUrl: https://github.com/TheSylence/ArchitectureAnalyzer -->
