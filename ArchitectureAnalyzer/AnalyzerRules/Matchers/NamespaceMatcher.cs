using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class NamespaceMatcher : Matcher
{
	public string Namespace { get; set; } = default!;

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (string.IsNullOrWhiteSpace(Namespace))
			return false;

		var namespaceName = Namespace;
		if (!namespaceName.StartsWith("global::"))
			namespaceName = "global::" + namespaceName;

		var regex = new Regex(
			"^" + Regex.Escape(namespaceName).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
			RegexOptions.IgnoreCase | RegexOptions.Singleline
		);

		return regex.IsMatch(symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
	}
}