using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NameMatcher : Matcher
{
	public string Name { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Name))
			return false;

		var regex = new Regex(
			"^" + Regex.Escape(Name).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
			RegexOptions.IgnoreCase | RegexOptions.Singleline
		);

		return regex.IsMatch(symbol.Name);
	}

	public override string ToString() => $"Name: {Name}";
}