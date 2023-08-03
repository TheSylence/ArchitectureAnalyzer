using LightJson;

namespace ArchitectureAnalyzer.AnalyzerRules;

internal abstract class JsonReader
{
	protected static string ReadSingleKey(JsonObject rule)
	{
		var keyValuePairs = (IEnumerable<KeyValuePair<string, JsonValue>>)rule;

		return keyValuePairs.Single().Key;
	}
}