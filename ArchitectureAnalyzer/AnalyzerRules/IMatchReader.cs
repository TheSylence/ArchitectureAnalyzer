using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using LightJson;

namespace ArchitectureAnalyzer.AnalyzerRules;

internal interface IMatchReader
{
	Matcher ReadMatcher(JsonObject matcher);
}