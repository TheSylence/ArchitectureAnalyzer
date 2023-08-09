using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class RelatedTypeExistsRuleTests
{
	public sealed class Required
	{
		private readonly RelatedTypeExistsRule _sut = new();

		[Fact]
		public void DoesNotViolate_WhenNoTypeMatches()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");

			var symbol = new SymbolBuilder().Build();

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeExists()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class RelatedA{}");

			var symbol = new SymbolBuilder().WithName("A").Build();

			_sut.ForTypes = new NameMatcher { Name = "A" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenTypeDoesNotExist()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");

			var symbol = new SymbolBuilder().WithName("A").Build();

			_sut.ForTypes = new NameMatcher { Name = "A" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}

	public sealed class Forbidden
	{
		private readonly RelatedTypeExistsRule _sut = new();

		[Fact]
		public void DoesNotViolate_WhenTypeDoesNotExist()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");

			var symbol = new SymbolBuilder().WithName("A").Build();

			_sut.ForTypes = new NameMatcher { Name = "A" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenNoTypeMatches()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");

			var symbol = new SymbolBuilder().Build();

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenTypeExists()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class RelatedA{}");

			var symbol = new SymbolBuilder().WithName("A").Build();

			_sut.ForTypes = new NameMatcher { Name = "A" };
			_sut.RelatedType = new NameMatcher { Name = "Related%type.Name%" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}
}