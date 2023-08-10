using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer;

internal static class Diagnostics
{
	public static IEnumerable<DiagnosticDescriptor> All()
	{
		yield return MustImplement;
		yield return MustInherit;
		yield return MustNotImplement;
		yield return MustNotInherit;
		yield return RelatedTypeExists;
		yield return RelatedTypeMustNotExist;
		yield return MustReference;
		yield return MustNotReference;
	}

	public static readonly DiagnosticDescriptor MustImplement = new(
		"AA0001",
		"Does not implement required interface",
		"Type '{0}' does not implement '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor MustInherit = new(
		"AA0002",
		"Does not inherit required type",
		"Type '{0}' does not inherit '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor MustNotImplement = new(
		"AA0003",
		"Implements forbidden interfaces",
		"Type '{0}' implements '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor MustNotInherit = new(
		"AA0004",
		"Inherits forbidden type",
		"Type '{0}' inherits '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor MustNotReference = new(
		"AA0008",
		"References forbidden type",
		"Type '{0}' references '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor MustReference = new(
		"AA0007",
		"Does not reference required type",
		"Type '{0}' does not reference '{1}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor RelatedTypeExists = new(
		"AA0005",
		"Related type does not exist",
		"Type '{1}' does not exist for '{0}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor RelatedTypeMustNotExist = new(
		"AA0006",
		"Related type exists",
		"Type '{1}' exists for '{0}'",
		"Design",
		DiagnosticSeverity.Warning,
		true);
}