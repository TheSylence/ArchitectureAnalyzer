using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class FullNameMatcher : Matcher
{
	public string FullName { get; set; } = default!;
	
	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(FullName))
			return false;

		var fullName = FullName;
		if(!fullName.StartsWith("global::"))
			fullName = "global::" + fullName;

		var regex = new Regex(
			"^" + Regex.Escape(fullName).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
			RegexOptions.IgnoreCase | RegexOptions.Singleline
		);

		return regex.IsMatch(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
	}

	public override string ToString() => $"FullName: {FullName}";
}