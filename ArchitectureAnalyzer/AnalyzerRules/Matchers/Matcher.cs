using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal abstract class Matcher
{
	public abstract bool Matches(INamedTypeSymbol symbol);
}