using ArchitectureAnalyzer.AnalyzerRules.Rules;
using LightJson;

namespace ArchitectureAnalyzer.AnalyzerRules;

internal sealed class RuleReader : JsonReader
{
	public RuleReader(IMatchReader matchReader)
	{
		_matchReader = matchReader;
	}

	public IEnumerable<Rule> Read(string json)
	{
		var root = JsonValue.Parse(json);

		var rules = root["rules"].AsJsonArray;
		if (rules is null)
			yield break;

		foreach (var rule in rules.Select(x => x.AsJsonObject))
		{
			if (rule is null)
				continue;

			var ruleType = ReadSingleKey(rule);
			var ruleData = rule[ruleType].AsJsonObject;
			if (ruleData is null)
				continue;

			yield return ruleType switch
			{
				"mustImplement" => ReadMustImplementRule(ruleData),
				"mustInherit" => ReadMustInheritRule(ruleData),
				"relatedTypeExists" => ReadRelatedTypeExistsRule(ruleData),
				_ => throw new NotSupportedException($"Unknown rule type '{ruleType}'.")
			};
		}
	}

	private static string ReadDescription(JsonObject rule) => rule["description"].AsString ?? string.Empty;

	private static bool ReadForbidden(JsonObject rule) => rule.ContainsKey("forbidden") && rule["forbidden"].AsBoolean;

	private MustImplementRule ReadMustImplementRule(JsonObject rule)
	{
		var forTypes = rule["forTypes"].AsJsonObject;
		if (forTypes is null)
			throw new AnalyzerException("MustImplement rule must have a forTypes matcher.");

		var interfaceMatcher = rule["interface"].AsJsonObject;
		if (interfaceMatcher is null)
			throw new AnalyzerException("MustImplement rule must have a interface matcher.");

		return new MustImplementRule
		{
			ForTypes = _matchReader.ReadMatcher(forTypes),
			Interface = _matchReader.ReadMatcher(interfaceMatcher),
			Forbidden = ReadForbidden(rule),
			Description = ReadDescription(rule)
		};
	}

	private MustInheritRule ReadMustInheritRule(JsonObject rule)
	{
		var forTypes = rule["forTypes"].AsJsonObject;
		if (forTypes is null)
			throw new AnalyzerException("MustInherit rule must have a forTypes matcher.");

		var baseType = rule["baseType"].AsJsonObject;
		if (baseType is null)
			throw new AnalyzerException("MustInherit rule must have a baseType matcher.");

		return new MustInheritRule
		{
			ForTypes = _matchReader.ReadMatcher(forTypes),
			BaseType = _matchReader.ReadMatcher(baseType),
			Forbidden = ReadForbidden(rule),
			Description = ReadDescription(rule)
		};
	}

	private RelatedTypeExistsRule ReadRelatedTypeExistsRule(JsonObject rule)
	{
		var forTypes = rule["forTypes"].AsJsonObject;
		if (forTypes is null)
			throw new AnalyzerException("RelatedTypeExists rule must have a forTypes matcher.");

		var relatedType = rule["relatedType"].AsJsonObject;
		if (relatedType is null)
			throw new AnalyzerException("RelatedTypeExists rule must have a relatedType matcher.");

		return new RelatedTypeExistsRule
		{
			ForTypes = _matchReader.ReadMatcher(forTypes),
			RelatedType = _matchReader.ReadMatcher(relatedType),
			Forbidden = ReadForbidden(rule),
			Description = ReadDescription(rule)
		};
	}

	private readonly IMatchReader _matchReader;
}