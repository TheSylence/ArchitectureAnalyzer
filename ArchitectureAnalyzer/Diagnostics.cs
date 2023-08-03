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
}