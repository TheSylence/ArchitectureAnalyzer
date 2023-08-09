using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace ArchitectureAnalyzer;

internal static class RegexCache
{
	public static Regex Cache(string pattern)
	{
		return Regexes.GetOrAdd(pattern, p => new Regex(p, RegexOptions.IgnoreCase | RegexOptions.Singleline));
	}

	private static readonly ConcurrentDictionary<string, Regex> Regexes = new();
}