# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Forbidden Property to rules to replace must-not-x rules
- Description property to document rules
- IsMatcher for matching types based on kind, accessibility or modifiers

### Changed
- Removed MustNotImplement and MustNotInherit. Replaced by forbidden property on rule

### Fixed
- Inherits matcher not matching for inherited base type

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


[0.2]: https://github.com/TheSylence/ArchitectureAnalyzer/compare/0.1...0.2
[0.1]: https://github.com/TheSylence/ArchitectureAnalyzer/releases/tag/0.1
<!-- Release: %URL%/releases/tag/%VERSION% -->
<!-- Compare: %URL%/compare/%OLD_VERSION%...%NEW_VERSION% -->
<!-- BaseUrl: https://github.com/TheSylence/ArchitectureAnalyzer -->
