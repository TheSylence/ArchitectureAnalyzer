using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Tests.Helper;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules;

public sealed class MatchReplacerTests
{
	private readonly NameMatcher _nameMatcher = new() { Name = "%type.Name%" };

	[Fact]
	public void Replaces_AndMatcher()
	{
		// Arrange
		var matcher = new AndMatcher
		{
			Matchers = new List<Matcher>
			{
				_nameMatcher
			}
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<AndMatcher>().Which.Matchers.Should().HaveCount(1)
			.And.ContainSingle(x => x is NameMatcher && x.As<NameMatcher>().Name == "Name");
	}

	[Fact]
	public void Replaces_FullNameMatcher()
	{
		// Arrange
		var matcher = new FullNameMatcher
		{
			FullName = "%type.FullName%"
		};

		var symbol = new SymbolBuilder().WithFullName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<FullNameMatcher>().Which.FullName.Should().Be("Name");
	}

	[Fact]
	public void Replaces_GenericMatcher()
	{
		// Arrange
		var matcher = new GenericMatcher
		{
			Type = _nameMatcher,
			TypeArguments = new Matcher[] { _nameMatcher }
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<GenericMatcher>()
			.Which.Type.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("Name");

		result.As<GenericMatcher>().TypeArguments.Should().HaveCount(1)
			.And.ContainSingle(x => x is NameMatcher && x.As<NameMatcher>().Name == "Name");
	}

	[Fact]
	public void Replaces_HasAttributeMatcher()
	{
		// Arrange
		var matcher = new HasAttributeMatcher
		{
			Attribute = _nameMatcher
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<HasAttributeMatcher>().Which.Attribute.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("Name");
	}

	[Fact]
	public void Replaces_ImplementsMatcher()
	{
		// Arrange
		var matcher = new ImplementsMatcher
		{
			Type = _nameMatcher
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<ImplementsMatcher>().Which.Type.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("Name");
	}

	[Fact]
	public void Replaces_InheritsMatcher()
	{
		// Arrange
		var matcher = new InheritsMatcher
		{
			Type = _nameMatcher
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<InheritsMatcher>().Which.Type.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("Name");
	}

	[Fact]
	public void Replaces_NameMatcher()
	{
		// Arrange
		var matcher = new NameMatcher
		{
			Name = "%type.Name%"
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("Name");
	}

	[Fact]
	public void Replaces_Nested_GenericMatcher()
	{
		// Arrange
		var innerMatcher = new GenericMatcher
		{
			Type = _nameMatcher,
			TypeArguments = new Matcher[] { _nameMatcher }
		};

		var outerMatcher = new ImplementsMatcher
		{
			Type = innerMatcher
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(outerMatcher, symbol);

		// Assert
		result.Should().BeOfType<ImplementsMatcher>()
			.Which.Type.Should().BeOfType<GenericMatcher>().Which.Type.Should().BeOfType<NameMatcher>().Which.Name
			.Should().Be("Name");

		result.Should().BeOfType<ImplementsMatcher>()
			.Which.Type.Should().BeOfType<GenericMatcher>().Which.TypeArguments.Should()
			.ContainSingle(x => x is NameMatcher && x.As<NameMatcher>().Name == "Name");
	}

	[Fact]
	public void Replaces_OrMatcher()
	{
		// Arrange
		var matcher = new OrMatcher
		{
			Matchers = new List<Matcher>
			{
				_nameMatcher
			}
		};

		var symbol = new SymbolBuilder().WithName("Name").Build();

		// Act
		var result = MatchReplacer.Replace(matcher, symbol);

		// Assert
		result.Should().BeOfType<OrMatcher>().Which.Matchers.Should().HaveCount(1)
			.And.ContainSingle(x => x is NameMatcher && x.As<NameMatcher>().Name == "Name");
	}
}