using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class HasAttributeMatcherTests
{
	private readonly HasAttributeMatcher _sut = new();

	[Fact]
	public void DoesNotMatch_WhenAttributeDoesNotMatch()
	{
		// Arrange
		_sut.Attribute = new NameMatcher { Name = "TestAttribute" };
		var symbol = new SymbolBuilder().WithAttribute("OtherAttribute").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenNoAttribute()
	{
		// Arrange
		_sut.Attribute = new NameMatcher { Name = "TestAttribute" };
		var symbol = new SymbolBuilder().WithNoAttributes().Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenAttributeMatches()
	{
		// Arrange
		_sut.Attribute = new NameMatcher { Name = "TestAttribute" };
		var symbol = new SymbolBuilder().WithAttribute("TestAttribute").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.Attribute = new NameMatcher { Name = "TestAttribute" };

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("HasAttribute: {Name: TestAttribute}");
	}
}