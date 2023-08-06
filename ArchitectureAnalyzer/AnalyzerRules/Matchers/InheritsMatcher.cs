using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class InheritsMatcher : Matcher
{
	public Matcher Type { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (symbol.BaseType is null)
			return false;

		var baseType = symbol.BaseType;

		while (!Type.Matches(baseType))
		{
			baseType = baseType.BaseType;
			if (baseType is null || baseType.SpecialType != SpecialType.None)
				return false;
		}

		return true;
	}

	public override string ToString() => $"Inherits: {{{Type}}}";
}