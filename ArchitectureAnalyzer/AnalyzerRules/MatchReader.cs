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
			"generic" => ReadGeneric(matcher),
			"implements" => ReadImplements(matcher),
			"inherits" => ReadInherits(matcher),
			"is" => ReadIs(matcher),
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

	private GenericMatcher ReadGeneric(JsonObject matcher)
	{
		var inner = matcher["generic"].AsJsonObject;
		if (inner is null)
			throw new AnalyzerException("Generic matcher must have a matcher as child.");

		var type = inner["type"].AsJsonObject;
		if (type is null)
			throw new AnalyzerException("Generic matcher must have a type as child.");

		var typeArguments = inner["typeArguments"].AsJsonArray;
		if (typeArguments is null)
			throw new AnalyzerException("Generic matcher must have type arguments as child.");

		var typeMatcher = ReadMatcher(type);
		var typeArgumentMatchers = new List<Matcher>();
		foreach (var typeArgument in typeArguments)
		{
			var typeArgumentMatcher = ReadMatcher(typeArgument.AsJsonObject);
			typeArgumentMatchers.Add(typeArgumentMatcher);
		}

		return new GenericMatcher
		{
			Type = typeMatcher,
			TypeArguments = typeArgumentMatchers.ToArray()
		};
	}

	private ImplementsMatcher ReadImplements(JsonObject matcher)
	{
		var inner = matcher["implements"].AsJsonObject;
		if (inner is null)
			throw new AnalyzerException("Implements matcher must have a matcher as child.");

		var typeMatcher = ReadMatcher(inner);

		return new ImplementsMatcher
		{
			Type = typeMatcher
		};
	}

	private InheritsMatcher ReadInherits(JsonObject matcher)
	{
		var inner = matcher["inherits"].AsJsonObject;
		if (inner is null)
			throw new AnalyzerException("Inherits matcher must have a matcher as child.");

		var typeMatcher = ReadMatcher(inner);

		return new InheritsMatcher
		{
			Type = typeMatcher
		};
	}

	private IsMatcher ReadIs(JsonObject matcher)
	{
		var kindString = matcher["is"].AsString;

		var kind = kindString.Split(',').Select(x =>
				Enum.TryParse<IsKind>(x.Trim(), true, out var kind)
					? kind
					: throw new AnalyzerException($"Unknown IsKind '{x}'."))
			.Aggregate(IsKind.None, (kind, isKind) => kind | isKind);

		return new IsMatcher
		{
			Kind = kind
		};
	}

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