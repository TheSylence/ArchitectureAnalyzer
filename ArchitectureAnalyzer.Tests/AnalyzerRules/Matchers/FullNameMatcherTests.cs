using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class FullNameMatcherTests
{
	private readonly FullNameMatcher _sut = new();
	private readonly INamedTypeSymbol _symbol = Substitute.For<INamedTypeSymbol>();

	[Fact]
	public void DoesNotMatch_WhenNameDoesNotMatch()
	{
		// Arrange
		_sut.FullName = "Test";

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData("Test", "Test1*")]
	[InlineData("Test", "*Test1")]
	[InlineData("Test", "*Test1*")]
	public void DoesNotMatch_whenNameDoesNotMatchWithWildcard(string name, string wildcard)
	{
		// Arrange
		_sut.FullName = wildcard;
		_symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns(name);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenNameIsEmpty()
	{
		// Arrange
		_sut.FullName = string.Empty;

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenNameMatches()
	{
		// Arrange
		_sut.FullName = "Test";
		_symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::Test");

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenNameMatchesCaseInsensitive()
	{
		// Arrange
		_sut.FullName = "Test";
		_symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::test");

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Theory]
	[InlineData("Test", "Test*")]
	[InlineData("Test", "*Test")]
	[InlineData("Test", "*Test*")]
	[InlineData("Test123", "Test*")]
	public void Matches_WhenNameMatchesWithWildcard(string name, string wildcard)
	{
		// Arrange
		_sut.FullName = wildcard;
		_symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns($"global::{name}");

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.FullName = "Test";

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("FullName: Test");
	}
}