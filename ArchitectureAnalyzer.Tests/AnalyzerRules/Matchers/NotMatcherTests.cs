using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class NotMatcherTests
{
	private readonly NotMatcher _sut = new();
	private readonly INamedTypeSymbol _symbol = Substitute.For<INamedTypeSymbol>();

	[Fact]
	public void DoesNotMatch_WhenMatcherMatches()
	{
		// Arrange
		_sut.Matcher = MockMatcher.Create(true, _symbol);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenMatcherDoesNotMatch()
	{
		// Arrange
		var nestedMatcher = Substitute.For<Matcher>();
		nestedMatcher.Matches(_symbol).Returns(false);

		_sut.Matcher = MockMatcher.Create(false, _symbol);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.Matcher = new NameMatcher { Name = "Test" };

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("Not: {Name: Test}");
	}
}