using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class InheritsMatcherTests
{
	private readonly InheritsMatcher _sut = new();

	[Fact]
	public void DoesMatch_WhenBaseTypeIsIndirectlyInherited()
	{
		// Arrange
		var parentSymbol = new SymbolBuilder().WithBaseType("System.Object").Build();
		var childSymbol = new SymbolBuilder().WithBaseType(parentSymbol).Build();
		var symbol = new SymbolBuilder().WithBaseType(childSymbol).Build();

		_sut.Type = MockMatcher.Create(true, parentSymbol);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void DoesNotMatch_WhenBaseTypeDoesNotMatch()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(false);
		var symbol = new SymbolBuilder().WithBaseType("System.Object").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}
	
	[Fact]
	public void DoesNotMatch_WhenBaseTypeIsObject()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(false);
		var symbol = new SymbolBuilder().WithSpecialType(SpecialType.System_Object).Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenNoBaseType()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(false);
		var symbol = new SymbolBuilder().WithNoBaseType().Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenBaseTypeMatches()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(true);
		var symbol = new SymbolBuilder().WithBaseType("System.Object").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.Type = new NameMatcher { Name = "Test" };

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("Inherits: {Name: Test}");
	}
}