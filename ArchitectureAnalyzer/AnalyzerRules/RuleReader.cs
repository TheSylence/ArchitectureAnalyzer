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
				"mustNotImplement" => ReadMustNotImplementRule(ruleData),
				"mustNotInherit" => ReadMustNotInheritRule(ruleData),
				_ => throw new NotSupportedException($"Unknown rule type '{ruleType}'.")
			};
		}
	}

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
			Interface = _matchReader.ReadMatcher(interfaceMatcher)
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
			BaseType = _matchReader.ReadMatcher(baseType)
		};
	}

	private MustNotImplementRule ReadMustNotImplementRule(JsonObject rule)
	{
		var forTypes = rule["forTypes"].AsJsonObject;
		if (forTypes is null)
			throw new AnalyzerException("MustNotImplement rule must have a forTypes matcher.");

		var interfaceMatcher = rule["interface"].AsJsonObject;
		if (interfaceMatcher is null)
			throw new AnalyzerException("MustNotImplement rule must have a interface matcher.");

		return new MustNotImplementRule
		{
			ForTypes = _matchReader.ReadMatcher(forTypes),
			Interface = _matchReader.ReadMatcher(interfaceMatcher)
		};
	}

	private MustNotInheritRule ReadMustNotInheritRule(JsonObject rule)
	{
		var forTypes = rule["forTypes"].AsJsonObject;
		if (forTypes is null)
			throw new AnalyzerException("MustNotInherit rule must have a forTypes matcher.");

		var baseType = rule["baseType"].AsJsonObject;
		if (baseType is null)
			throw new AnalyzerException("MustNotInherit rule must have a baseType matcher.");

		return new MustNotInheritRule
		{
			ForTypes = _matchReader.ReadMatcher(forTypes),
			BaseType = _matchReader.ReadMatcher(baseType)
		};
	}

	private readonly IMatchReader _matchReader;
}