using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NameMatcher : Matcher
{
	public string Name { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Name))
			return false;

		return Name.Matches(symbol.Name);
	}

	public override string ToString() => $"Name: {Name}";
}