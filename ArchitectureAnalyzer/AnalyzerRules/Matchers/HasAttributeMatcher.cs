using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class HasAttributeMatcher : Matcher
{
	public Matcher Attribute { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		return symbol.GetAttributes().Any(attr =>
		{
			if (attr.AttributeClass is null)
				return false;

			return Attribute.Matches(attr.AttributeClass);
		});
	}

	public override string ToString() => $"HasAttribute: {{{Attribute}}}";
}