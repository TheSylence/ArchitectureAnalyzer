using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustInheritRuleTests
{
	public sealed class Required
	{
		private readonly MustInheritRule _sut = new();
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenTypeIsInherited()
		{
			// Arrange
			var baseType = new SymbolBuilder().Build();
			var symbol = new SymbolBuilder().WithBaseType(baseType).Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.ForTypes = forTypes;
			_sut.BaseType = MockMatcher.Create(true, baseType);

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

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
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenTypeIsNotInherited()
		{
			// Arrange
			var baseType = new SymbolBuilder().Build();
			var symbol = new SymbolBuilder().Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.ForTypes = forTypes;
			_sut.BaseType = MockMatcher.Create(true, baseType);

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}

	public sealed class Forbidden
	{
		private readonly MustInheritRule _sut = new();
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenTypeIsNotInherited()
		{
			// Arrange
			var baseType = new SymbolBuilder().Build();
			var symbol = new SymbolBuilder().Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.Forbidden = true;
			_sut.ForTypes = forTypes;
			_sut.BaseType = MockMatcher.Create(true, baseType);

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsNotMatched()
		{
			// Arrange
			var symbol = new SymbolBuilder().Build();
			var forTypes = MockMatcher.Create(false, symbol);

			_sut.Forbidden = true;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

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

			_sut.Forbidden = true;
			_sut.ForTypes = forTypes;
			_sut.BaseType = MockMatcher.Create(true, baseType);

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}
}