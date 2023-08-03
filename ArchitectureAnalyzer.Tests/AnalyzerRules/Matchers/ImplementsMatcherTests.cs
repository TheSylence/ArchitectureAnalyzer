using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class ImplementsMatcherTests
{
	private readonly ImplementsMatcher _sut = new();

	[Fact]
	public void DoesNotMatch_WhenImplementingNonMatchingInterfaces()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(false);
		var symbol = new SymbolBuilder().WithInterfaces("I1", "I2").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
		_sut.Type.Received(2).Matches(Arg.Any<INamedTypeSymbol>());
	}

	[Fact]
	public void DoesNotMatch_WhenNotImplementingAnyInterfaces()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(false);
		var symbol = new SymbolBuilder().WithNoInterfaces().Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
		_sut.Type.DidNotReceive().Matches(Arg.Any<INamedTypeSymbol>());
	}

	[Fact]
	public void Matches_WhenImplementingMatchingInterface()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(true);
		var symbol = new SymbolBuilder().WithInterfaces("I1").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
		_sut.Type.Received(1).Matches(Arg.Any<INamedTypeSymbol>());
	}

	[Fact]
	public void Matches_WhenImplementingMatchingInterfaces()
	{
		// Arrange
		_sut.Type = MockMatcher.Create(true);
		var symbol = new SymbolBuilder().WithInterfaces("I1", "I2").Build();

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
		_sut.Type.Received(1).Matches(Arg.Any<INamedTypeSymbol>());
	}

	[Fact]
	public void ToString_ProducesDisplayString()
	{
		// Arrange
		_sut.Type = new NameMatcher { Name = "Test" };

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be("Implements: {Name: Test}");
	}
}