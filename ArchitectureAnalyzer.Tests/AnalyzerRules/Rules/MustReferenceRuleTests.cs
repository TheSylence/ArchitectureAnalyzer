using System.Diagnostics;
using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

public sealed class MustReferenceRuleTests
{
	public sealed class Required
	{
		private readonly MustReferenceRule _sut = new();

		[Fact]
		public void DoesNotViolate_WhenNoTypeMatches()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");
			var symbol = new SymbolBuilder().Build();

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsReferenced_ByConstructorParameter()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{ B(A a){}}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsReferenced_ByField()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{A a;}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsReferenced_ByLocalVariable()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{ void M(){ A a;}}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsReferenced_ByProperty()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{ A a{get;}}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenTypeIsNotReferenced()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}

	public sealed class Forbidden
	{
		private readonly MustReferenceRule _sut = new();

		[Fact]
		public void DoesNotViolate_WhenNoTypeMatches()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{}");
			var symbol = new SymbolBuilder().Build();

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void DoesNotViolate_WhenTypeIsNotReferenced()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{}");
			var symbol = compilation.GetTypeByMetadataName("A") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().BeNull();
		}

		[Fact]
		public void Violates_WhenTypeIsReferenced()
		{
			// Arrange
			var compilation = CompilationBuilder.Compile("class A{} class B{ B(A a){}}");
			var symbol = compilation.GetTypeByMetadataName("B") ?? throw new UnreachableException("Type not found");

			_sut.ForTypes = new NameMatcher { Name = "B" };
			_sut.Reference = new NameMatcher { Name = "A" };
			_sut.Forbidden = true;

			// Act
			var result = _sut.Evaluate(symbol, compilation);

			// Assert
			result.Should().NotBeNull();
		}
	}
}