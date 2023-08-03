using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class AndMatcher : Matcher
{
	public List<Matcher> Matchers { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		return Matchers.All(matcher => matcher.Matches(symbol));
	}

	public override string ToString()
	{
		var matchers = Matchers.Select(m => $"{{{m}}}");
		return $"And: [{string.Join(", ", matchers)}]";
	}
}