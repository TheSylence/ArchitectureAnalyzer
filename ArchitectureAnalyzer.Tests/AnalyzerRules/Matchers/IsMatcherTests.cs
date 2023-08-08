using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;

public sealed class IsMatcherTests
{
	private readonly IsMatcher _sut = new();
	private readonly INamedTypeSymbol _symbol = Substitute.For<INamedTypeSymbol>();

	[Fact]
	public void DoesNotMatch_WhenKindDoesNotMatch()
	{
		// Arrange
		_sut.Kind = IsKind.Class;
		_symbol.TypeKind.Returns(TypeKind.Interface);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeIsNotAbstract()
	{
		// Arrange
		_sut.Kind = IsKind.Abstract;
		_symbol.IsAbstract.Returns(false);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeIsNotSealed()
	{
		// Arrange
		_sut.Kind = IsKind.Sealed;
		_symbol.IsSealed.Returns(false);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void DoesNotMatch_WhenTypeIsNotStatic()
	{
		// Arrange
		_sut.Kind = IsKind.Static;
		_symbol.IsStatic.Returns(false);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData(Accessibility.Internal)]
	[InlineData(Accessibility.Private)]
	[InlineData(Accessibility.Protected)]
	public void DoesNotMatch_WhenAccessibilityDoesNotMatch(Accessibility accessibility)
	{
		// Arrange
		_sut.Kind = IsKind.Public;
		_symbol.DeclaredAccessibility.Returns(accessibility);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Matches_WhenAccessibilityMatches()
	{
		// Arrange
		_sut.Kind = IsKind.Public;
		_symbol.DeclaredAccessibility.Returns(Accessibility.Public);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenTypeIsAbstract()
	{
		// Arrange
		_sut.Kind = IsKind.Abstract;
		_symbol.IsAbstract.Returns(true);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenTypeIsSealed()
	{
		// Arrange
		_sut.Kind = IsKind.Sealed;
		_symbol.IsSealed.Returns(true);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenTypeIsStatic()
	{
		// Arrange
		_sut.Kind = IsKind.Static;
		_symbol.IsStatic.Returns(true);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Matches_WhenTypeKindMatches()
	{
		// Arrange
		_sut.Kind = IsKind.Class;
		_symbol.TypeKind.Returns(TypeKind.Class);

		// Act
		var result = _sut.Matches(_symbol);

		// Assert
		result.Should().BeTrue();
	}

	[Theory]
	[InlineData(IsKind.Public, "Is: public")]
	[InlineData(IsKind.Internal, "Is: internal")]
	[InlineData(IsKind.Protected, "Is: protected")]
	[InlineData(IsKind.Private, "Is: private")]
	[InlineData(IsKind.Abstract, "Is: abstract")]
	[InlineData(IsKind.Sealed, "Is: sealed")]
	[InlineData(IsKind.Static, "Is: static")]
	[InlineData(IsKind.Class, "Is: class")]
	[InlineData(IsKind.Interface, "Is: interface")]
	[InlineData(IsKind.Struct, "Is: struct")]
	[InlineData(IsKind.Enum, "Is: enum")]
	[InlineData(IsKind.Public|IsKind.Static, "Is: public static")]
	[InlineData(IsKind.Public|IsKind.Abstract, "Is: public abstract")]
	[InlineData(IsKind.Public|IsKind.Sealed, "Is: public sealed")]
	[InlineData(IsKind.Class|IsKind.Abstract, "Is: abstract class")]
	[InlineData(IsKind.Class|IsKind.Public|IsKind.Sealed, "Is: public sealed class")]
	public void ToString_ProducesDisplayString(IsKind kind, string expected)
	{
		// Arrange
		_sut.Kind = kind;

		// Act
		var result = _sut.ToString();

		// Assert
		result.Should().Be(expected);
	}
}