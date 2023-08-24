using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class GenericMatcher : Matcher
{
	public Matcher Type { get; set; } = default!;
	public Matcher[] TypeArguments { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (Type is GenericMatcher)
			throw new InvalidOperationException("GenericMatcher cannot be used as Type in GenericMatcher");

		if (!symbol.IsGenericType)
			return false;

		if (!Type.Matches(symbol))
			return false;

		var expectedTypeArguments = TypeArguments.Length;
		if (symbol.TypeArguments.Length != expectedTypeArguments)
			return false;

		for (var i = 0; i < expectedTypeArguments; i++)
		{
			if (symbol.TypeArguments[i] is not INamedTypeSymbol namedTypeSymbol)
				return false;

			if (!TypeArguments[i].Matches(namedTypeSymbol))
				return false;
		}

		return true;
	}

	public override string ToString()
	{
		var typeArgs = string.Join(",", TypeArguments.Select(x => $"{{{x}}}"));
		return $"Generic: {{{Type}}}<{typeArgs}>";
	}
}