using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class OrMatcherTests
{
	private readonly OrMatcher _sut = new();
	private readonly INamedTypeSymbol _symbol = Substitute.For<INamedTypeSymbol>();

	[Fact]
	public void DoesNotMatch_WhenAllMatchersDoNotMatch()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(false, _symbol)
		};

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenNoMatcherMatches()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(false, _symbol)
		};

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenAllMatchersMatch()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			MockMatcher.Create(true, _symbol),
			MockMatcher.Create(true, _symbol),
			MockMatcher.Create(true, _symbol)
		};

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenMultipleMatchersMatch()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(true, _symbol),
			MockMatcher.Create(true, _symbol)
		};

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenOneMatcherMatches()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			MockMatcher.Create(false, _symbol),
			MockMatcher.Create(true, _symbol),
			MockMatcher.Create(false, _symbol)
		};

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.Matchers = new List<Matcher>
		{
			new NameMatcher { Name = "Test" },
			new NameMatcher { Name = "Test1" },
			new NameMatcher { Name = "Test2" }
		};

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("Or: [{Name: Test}, {Name: Test1}, {Name: Test2}]");
	}
}