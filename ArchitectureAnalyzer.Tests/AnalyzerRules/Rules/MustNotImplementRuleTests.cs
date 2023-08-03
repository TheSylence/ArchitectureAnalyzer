using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustNotImplementRuleTests
{
	private readonly MustNotImplementRule _sut = new();

	[Fact]
	public void DoesNotViolate_WhenInterfaceIsNotImplemented()
	{
		// Arrange
		var @interface = new SymbolBuilder().Build();
		var interfaceMatcher = MockMatcher.Create(true, @interface);
		var symbol = new SymbolBuilder().WithNoInterfaces().Build();
		var forTypes = MockMatcher.Create(true, symbol);

		_sut.Interface = interfaceMatcher;
		_sut.ForTypes = forTypes;

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public void DoesNotViolate_WhenTypeIsNotMatched()
	{
		// Arrange
		var @interface = new SymbolBuilder().Build();
		var interfaceMatcher = MockMatcher.Create(true, @interface);
		var symbol = new SymbolBuilder().Build();
		var forTypes = MockMatcher.Create(false, symbol);

		_sut.Interface = interfaceMatcher;
		_sut.ForTypes = forTypes;

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public void Violates_WhenInterfaceIsImplemented()
	{
		// Arrange
		var @interface = new SymbolBuilder().Build();
		var interfaceMatcher = MockMatcher.Create(true, @interface);
		var symbol = new SymbolBuilder().WithInterfaces(@interface).Build();
		var forTypes = MockMatcher.Create(true, symbol);

		_sut.Interface = interfaceMatcher;
		_sut.ForTypes = forTypes;

		// Act
		var result = _sut.Evaluate(symbol);

		// Assert
		result.Should().NotBeNull();
	}
}