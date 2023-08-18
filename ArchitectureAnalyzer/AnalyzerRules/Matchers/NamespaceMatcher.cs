using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NamespaceMatcher : Matcher
{
	public string Namespace { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Namespace))
			return false;

		var namespaceName = Namespace.Prefix("global::");

		return namespaceName.Matches(symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
	}

	public override string ToString() => $"Namespace: {Namespace}";
}