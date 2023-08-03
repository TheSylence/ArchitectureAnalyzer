using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

internal static class MockMatcher
{
	public static Matcher Create(bool matches, INamedTypeSymbol symbol)
	{
		var matcher = Substitute.For<Matcher>();
		matcher.Matches(symbol).Returns(matches);

		return matcher;		
	}
	
	public static Matcher Create(bool matches)
	{
		var matcher = Substitute.For<Matcher>();
		matcher.Matches(Arg.Any<INamedTypeSymbol>()).Returns(matches);

		return matcher;
	}
}