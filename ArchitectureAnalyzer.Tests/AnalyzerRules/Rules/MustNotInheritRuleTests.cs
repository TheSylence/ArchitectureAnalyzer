using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustNotInheritRuleTests
{
	private readonly MustNotInheritRule _sut = new();
	
	[Fact]
	public void DoesNotViolate_WhenTypeIsNotInherited()
	{
		// Arrange
		var baseType = new SymbolBuilder().Build();
		var symbol = new SymbolBuilder().Build();
		var forTypes = MockMatcher.Create(true, symbol);

		_sut.ForTypes = forTypes;
		_sut.BaseType = MockMatcher.Create(true, baseType);

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().BeNull();
	}
	
	[Fact]
	public void DoesNotViolate_WhenTypeIsNotMatched()
	{
		// Arrange
		var symbol = new SymbolBuilder().Build();
		var forTypes = MockMatcher.Create(false, symbol);

		_sut.ForTypes = forTypes;

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().BeNull();
	}
	
	[Fact]
	public void Violates_WhenTypeIsInherited()
	{
		// Arrange
		var baseType = new SymbolBuilder().Build();
		var symbol = new SymbolBuilder().WithBaseType(baseType).Build();
		var forTypes = MockMatcher.Create(true, symbol);

		_sut.ForTypes = forTypes;
		_sut.BaseType = MockMatcher.Create(true, baseType);

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().NotBeNull();
	}
}