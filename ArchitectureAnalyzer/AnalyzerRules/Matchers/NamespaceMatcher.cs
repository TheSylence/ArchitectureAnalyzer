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

		var wildcard = Namespace.Prefix("global::");
		var namespaceName = symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		return wildcard.Matches(namespaceName);
	}

	public override string ToString() => $"Namespace: {Namespace}";
}