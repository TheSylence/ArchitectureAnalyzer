using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class ImplementsMatcher : Matcher
{
	public Matcher Type { get; set; } = default!;

	public override string ToString() => $"Implements: {{{Type}}}";

	public override bool Matches(INamedTypeSymbol symbol)
	{
		return Enumerable.Any(symbol.AllInterfaces, @interface => Type.Matches(@interface));
	}
}