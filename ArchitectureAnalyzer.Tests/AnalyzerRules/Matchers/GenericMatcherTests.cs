using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class GenericMatcherTests
{
	private readonly GenericMatcher _sut = new();

	[Fact]
	public void DoesNotMatch_WhenAnyTypeArgumentDoesNotMatch()
	{
		// Arrange
		var symbol = new SymbolBuilder().WithGenericArguments(new SymbolBuilder().Build(), new SymbolBuilder().Build())
			.Build();

		_sut.Type = Substitute.For<Matcher>();
		_sut.Type.Matches(symbol).Returns(true);

		_sut.TypeArguments = new[] { Substitute.For<Matcher>(), Substitute.For<Matcher>() };
		_sut.TypeArguments[0].Matches(symbol).Returns(true);
		_sut.TypeArguments[1].Matches(symbol).Returns(false);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeArgumentCountDoesNotMatch()
	{
		// Arrange
		var symbol = new SymbolBuilder().WithGenericArguments(new SymbolBuilder().Build(), new SymbolBuilder().Build())
			.Build();

		_sut.Type = Substitute.For<Matcher>();
		_sut.Type.Matches(symbol).Returns(true);

		_sut.TypeArguments = new[] { Substitute.For<Matcher>() };
		_sut.TypeArguments[0].Matches(symbol).Returns(true);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeArgumentDoNotMatch()
	{
		// Arrange
		var symbol = new SymbolBuilder().WithGenericArguments(new SymbolBuilder().Build()).Build();

		_sut.Type = Substitute.For<Matcher>();
		_sut.Type.Matches(symbol).Returns(true);

		_sut.TypeArguments = new[] { Substitute.For<Matcher>() };
		_sut.TypeArguments[0].Matches(symbol).Returns(false);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeDoesNotMatch()
	{
		// Arrange
		var symbol = new SymbolBuilder().Build();

		_sut.Type = Substitute.For<Matcher>();
		_sut.Type.Matches(symbol).Returns(false);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenTypeAndTypeArgumentMatch()
	{
		// Arrange
		var genericArgument = new SymbolBuilder().Build();
		var symbol = new SymbolBuilder().WithGenericArguments(genericArgument).Build();

		_sut.Type = Substitute.For<Matcher>();
		_sut.Type.Matches(symbol).Returns(true);

		_sut.TypeArguments = new[] { Substitute.For<Matcher>() };
		_sut.TypeArguments[0].Matches(genericArgument).Returns(true);

		// Act
		var result = _sut.Matches(symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Throws_WhenNestedAsTypeMatcher()
	{
		// Arrange
		var symbol = new SymbolBuilder().Build();
		_sut.Type = new GenericMatcher();

		// Act
		var action = () => _sut.Matches(symbol);

		// Assert
		action.Should().Throw<InvalidOperationException>();
	}
}