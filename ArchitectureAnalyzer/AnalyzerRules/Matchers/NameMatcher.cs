using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NameMatcher : WildcardMatcher
{
	public string Name { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Name))
			return false;

		return Matches(symbol.Name, Name);
	}

	public override string ToString() => $"Name: {Name}";
}