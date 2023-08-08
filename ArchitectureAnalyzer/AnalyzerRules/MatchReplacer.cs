using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules;

internal static class MatchReplacer
{
	public static Matcher Replace(Matcher matcher, INamedTypeSymbol type)
	{
		return matcher switch
		{
			NameMatcher name => Replace(name, type),
			FullNameMatcher fullName => Replace(fullName, type),
			AndMatcher and => Replace(and, type),
			OrMatcher or => Replace(or, type),
			HasAttributeMatcher hasAttribute => Replace(hasAttribute, type),
			ImplementsMatcher implements => Replace(implements, type),
			InheritsMatcher inherits => Replace(inherits, type),
			GenericMatcher generic => Replace(generic, type),
			_ => throw new NotSupportedException($"Unknown matcher type '{matcher.GetType().Name}'.")
		};
	}

	private static HasAttributeMatcher Replace(HasAttributeMatcher matcher, INamedTypeSymbol type) => new()
	{
		Attribute = Replace(matcher.Attribute, type)
	};

	private static ImplementsMatcher Replace(ImplementsMatcher matcher, INamedTypeSymbol type) => new()
	{
		Type = Replace(matcher.Type, type)
	};

	private static InheritsMatcher Replace(InheritsMatcher matcher, INamedTypeSymbol type) => new()
	{
		Type = Replace(matcher.Type, type)
	};

	private static GenericMatcher Replace(GenericMatcher matcher, INamedTypeSymbol type) => new()
	{
		Type = Replace(matcher.Type, type),
		TypeArguments = matcher.TypeArguments.Select(m => Replace(m, type)).ToArray()
	};

	private static AndMatcher Replace(AndMatcher matcher, INamedTypeSymbol type) => new()
	{
		Matchers = matcher.Matchers.Select(m => Replace(m, type)).ToList()
	};

	private static OrMatcher Replace(OrMatcher matcher, INamedTypeSymbol type) => new()
	{
		Matchers = matcher.Matchers.Select(m => Replace(m, type)).ToList()
	};

	private static string Replace(string matcher, INamedTypeSymbol type) => matcher
		.Replace("%type.Name%", type.Name)
		.Replace("%type.FullName%", type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
		.Replace("%type.Namespace%",
			type.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

	private static FullNameMatcher Replace(FullNameMatcher matcher, INamedTypeSymbol type) => new()
	{
		FullName = Replace(matcher.FullName, type)
	};

	private static NameMatcher Replace(NameMatcher matcher, INamedTypeSymbol type) => new()
	{
		Name = Replace(matcher.Name, type)
	};
}