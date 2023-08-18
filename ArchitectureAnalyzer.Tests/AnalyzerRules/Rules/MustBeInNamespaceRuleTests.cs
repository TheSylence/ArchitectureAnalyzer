using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustBeInNamespaceRuleTests
{
	public sealed class Required
	{
		private readonly MustBeInNamespaceRule _sut = new();
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenTypeIsInNamespace()
		{
			// Arrange
			var symbol = new SymbolBuilder().WithNamespace("global::Name.Space").Build();

			_sut.ForTypes = MockMatcher.Create(true);
			_sut.Namespace = "Name.Space";

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
		public void Violates_WhenTypeIsNotInNamespace()
		{
			// Arrange
			var symbol = new SymbolBuilder().WithNamespace("global::Name.Space").Build();

			_sut.ForTypes = MockMatcher.Create(true);
			_sut.Namespace = "Other.Name.Space";

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}

	public sealed class Forbidden
	{
		private readonly MustBeInNamespaceRule _sut = new() { Forbidden = true };
		private readonly Compilation _compilation = default!;

		[Fact]
		public void DoesNotViolate_WhenTypeIsNotInNamespace()
		{
			// Arrange
			var symbol = new SymbolBuilder().WithNamespace("global::Name.Space").Build();

			_sut.ForTypes = MockMatcher.Create(true);
			_sut.Namespace = "Other.Name.Space";

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
		public void Violates_WhenTypeIsInNamespace()
		{
			// Arrange
			var symbol = new SymbolBuilder().WithNamespace("global::Name.Space").Build();

			_sut.ForTypes = MockMatcher.Create(true);
			_sut.Namespace = "Name.Space";

			// Act
			var result = _sut.Evaluate(symbol, _compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}
}