using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class OrMatcher : Matcher
{
	public List<Matcher> Matchers { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		return Matchers.Any(matcher => matcher.Matches(symbol));
	}

	public override string ToString()
	{
		var matchers = Matchers.Select(m => $"{{{m}}}");
		return $"Or: [{string.Join(", ", matchers)}]";
	}
}