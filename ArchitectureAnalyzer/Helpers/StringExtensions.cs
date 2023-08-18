using System.Text.RegularExpressions;

namespace ArchitectureAnalyzer.Helpers;

internal static class StringExtensions
{
	public static string Prefix(this string value, string prefix)
	{
		if (!value.StartsWith(prefix))
			return prefix + value;

		return value;
	}
	
	
	public static bool Matches( this string wildcard, string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
			return false;

		var regex = RegexCache.Cache("^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");

		return regex.IsMatch(expression);
	}
}