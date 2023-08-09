using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class NamespaceMatcherTests
{
	private readonly NamespaceMatcher _sut = new();
	private readonly INamedTypeSymbol _symbol = Substitute.For<INamedTypeSymbol>();

	[Fact]
	public void DoesNotMatch_WhenNamespaceDoesNotMatch()
	{
		// Arrange
		_sut.Namespace = "Test";

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData("Test", "Test1*")]
	[InlineData("Test", "*Test1")]
	[InlineData("Test", "*Test1*")]
	public void DoesNotMatch_whenNamespaceDoesNotMatchWithWildcard(string ns, string wildcard)
	{
		// Arrange
		_sut.Namespace = wildcard;
		_symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::" + ns);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenNamespaceIsEmpty()
	{
		// Arrange
		_sut.Namespace = string.Empty;

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenNamespaceMatches()
	{
		// Arrange
		_sut.Namespace = "Test";
		_symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::Test");

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenNamespaceMatchesCaseInsensitive()
	{
		// Arrange
		_sut.Namespace = "Test";
		_symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::test");

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
	public void Matches_WhenNamespaceMatchesWithWildcard(string ns, string wildcard)
	{
		// Arrange
		_sut.Namespace = wildcard;
		_symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Returns("global::" + ns);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}
}