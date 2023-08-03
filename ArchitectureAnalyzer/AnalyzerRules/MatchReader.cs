using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using LightJson;

namespace ArchitectureAnalyzer.AnalyzerRules;

internal sealed class MatchReader : JsonReader, IMatchReader
{
	public Matcher ReadMatcher(JsonObject matcher)
	{
		var matcherType = ReadSingleKey(matcher);

		return matcherType switch
		{
			"name" => ReadName(matcher),
			"fullName" => ReadFullName(matcher),
			"not" => ReadNot(matcher),
			"or" => ReadOr(matcher),
			"and" => ReadAnd(matcher),
			_ => throw new NotSupportedException($"Unknown matcher type '{matcherType}'.")
		};
	}

	private AndMatcher ReadAnd(JsonObject matcher)
	{
		var inner = matcher["and"].AsJsonArray;
		if (inner is null)
			throw new AnalyzerException("And matcher must have a matcher as child.");

		var innerMatchers = new List<Matcher>();
		foreach (var m in inner)
		{
			var jsonObject = m.AsJsonObject;
			if (jsonObject is null)
				throw new AnalyzerException("Failed to read inner matcher.");

			innerMatchers.Add(ReadMatcher(jsonObject));
		}

		return new AndMatcher
		{
			Matchers = innerMatchers
		};
	}

	private FullNameMatcher ReadFullName(JsonObject matcher) => new()
	{
		FullName = matcher["fullName"].AsString
	};

	private NameMatcher ReadName(JsonObject matcher) => new()
	{
		Name = matcher["name"].AsString
	};

	private NotMatcher ReadNot(JsonObject matcher)
	{
		var innerMatcher = matcher["not"].AsJsonObject;
		if (innerMatcher is null)
			throw new AnalyzerException("Not matcher must have a matcher as child.");

		return new NotMatcher
		{
			Matcher = ReadMatcher(innerMatcher)
		};
	}

	private OrMatcher ReadOr(JsonObject matcher)
	{
		var inner = matcher["or"].AsJsonArray;
		if (inner is null)
			throw new AnalyzerException("Or matcher must have a matcher as child.");

		var innerMatchers = new List<Matcher>();
		foreach (var m in inner)
		{
			var jsonObject = m.AsJsonObject;
			if (jsonObject is null)
				throw new AnalyzerException("Failed to read inner matcher.");

			innerMatchers.Add(ReadMatcher(jsonObject));
		}

		return new OrMatcher
		{
			Matchers = innerMatchers
		};
	}
}