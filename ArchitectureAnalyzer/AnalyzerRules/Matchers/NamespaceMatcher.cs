using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NamespaceMatcher : WildcardMatcher
{
	public string Namespace { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Namespace))
			return false;

		var namespaceName = Prefix(Namespace, "global::");

		return Matches(symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
			namespaceName);
	}
}