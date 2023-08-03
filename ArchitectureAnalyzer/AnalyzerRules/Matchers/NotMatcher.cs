using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NotMatcher : Matcher
{
	public Matcher Matcher { get; set; } = default!;

	public override string ToString() => $"Not: {{{Matcher}}}";

	public override bool Matches(INamedTypeSymbol symbol) => !Matcher.Matches(symbol);
}