using System.Text.RegularExpressions;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal abstract class WildcardMatcher : Matcher
{
	protected static bool Matches(string expression, string wildcard)
	{
		if (string.IsNullOrWhiteSpace(expression))
			return false;

		var regex = RegexCache.Cache("^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");

		return regex.IsMatch(expression);
	}

	protected static string Prefix(string value, string prefix)
	{
		if (!value.StartsWith(prefix))
			return prefix + value;

		return value;
	}
}