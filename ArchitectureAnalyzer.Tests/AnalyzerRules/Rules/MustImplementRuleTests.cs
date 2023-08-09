using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustImplementRuleTests
{
	public sealed class Forbidden
	{
		private readonly MustImplementRule _sut = new();
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenInterfaceIsNotImplemented()
		{
			// Arrange
			var @interface = new SymbolBuilder().Build();
			var interfaceMatcher = MockMatcher.Create(true, @interface);
			var symbol = new SymbolBuilder().WithNoInterfaces().Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.Forbidden = true;
			_sut.Interface = interfaceMatcher;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

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

			_sut.Forbidden = true;
			_sut.Interface = interfaceMatcher;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

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

			_sut.Forbidden = true;
			_sut.Interface = interfaceMatcher;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}

	public sealed class Required
	{
		private readonly MustImplementRule _sut = new();
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenInterfaceIsImplemented()
		{
			// Arrange
			var @interface = new SymbolBuilder().Build();
			var interfaceMatcher = MockMatcher.Create(true, @interface);
			var symbol = new SymbolBuilder().WithInterfaces(@interface).Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.Interface = interfaceMatcher;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

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
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenInterfaceIsNotImplemented()
		{
			// Arrange
			var @interface = new SymbolBuilder().Build();
			var interfaceMatcher = MockMatcher.Create(true, @interface);
			var symbol = new SymbolBuilder().WithNoInterfaces().Build();
			var forTypes = MockMatcher.Create(true, symbol);

			_sut.Interface = interfaceMatcher;
			_sut.ForTypes = forTypes;

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}
}