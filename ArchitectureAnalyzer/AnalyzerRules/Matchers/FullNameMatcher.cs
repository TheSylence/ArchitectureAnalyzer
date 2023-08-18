using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class FullNameMatcher : Matcher
{
	public string FullName { get; set; } = default!;
	
	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(FullName))
			return false;

		var fullName = FullName.Prefix("global::");

		return fullName.Matches(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
	}

	public override string ToString() => $"FullName: {FullName}";
}