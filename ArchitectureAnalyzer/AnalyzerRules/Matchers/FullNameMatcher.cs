using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class FullNameMatcher : WildcardMatcher
{
	public string FullName { get; set; } = default!;
	
	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(FullName))
			return false;

		var fullName = Prefix(FullName, "global::");

		return Matches(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), fullName);
	}

	public override string ToString() => $"FullName: {FullName}";
}