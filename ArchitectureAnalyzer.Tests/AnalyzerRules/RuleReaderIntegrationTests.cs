using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using FluentAssertions.Execution;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules;

public sealed class RuleReaderIntegrationTests
{
	private readonly RuleReader _sut = new(new MatchReader());

	[Fact]
	public void Reads_CompleteRuleSet()
	{
		// Arrange
		const string json = """
		                    {
		                    "rules": [
		                    {
		                    "mustImplement": {
		                    "forTypes": {
		                    "and": [{ "name": "*MustImplementName" }, { "name": "Start*" }]
		                    },
		                    "interface": { "name": "MustImplementInterface" }
		                    }
		                    },
		                    {
		                    "mustInherit": {
		                    "forTypes": { "fullName": "MustInheritFullName" },
		                    "baseType": { "fullName": "MustInheritBaseType" }
		                    }
		                    },
		                    {
		                    "mustNotImplement": {
		                    "forTypes": {
		                    "generic": {
		                    "type": { "name": "GenericType" },
		                    "typeArguments": [{ "fullName": "System.String" }]
		                    }
		                    },
		                    "interface": { "name": "MustNotImplementInterface" }
		                    }
		                    },
		                    {
		                    "mustNotInherit": {
		                    "forTypes": { "not": { "name": "MustNotInheritName" } },
		                    "baseType": {
		                    "or": [
		                    { "name": "MustNotInheritBaseType" },
		                    { "fullName": "MustNotInheritFullBaseType" }
		                    ]
		                    }
		                    }
		                    }
		                    ]
		                    }

		                    """;

		// Act
		var rules = _sut.Read(json).ToList();

		// Assert
		using var _ = new AssertionScope();

		rules.Should().HaveCount(4);

		rules[0].Should().BeOfType<MustImplementRule>();
		{
			var rule = rules[0].As<MustImplementRule>();

			rule.ForTypes.Should().BeOfType<AndMatcher>()
				.Which.Matchers.Should().HaveCount(2)
				.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "*MustImplementName")
				.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "Start*");

			rule.Interface.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustImplementInterface");
		}

		rules[1].Should().BeOfType<MustInheritRule>();
		{
			var rule = rules[1].As<MustInheritRule>();

			rule.ForTypes.Should().BeOfType<FullNameMatcher>()
				.Which.FullName.Should().Be("MustInheritFullName");

			rule.BaseType.Should().BeOfType<FullNameMatcher>()
				.Which.FullName.Should().Be("MustInheritBaseType");
		}

		rules[2].Should().BeOfType<MustNotImplementRule>();
		{
			var rule = rules[2].As<MustNotImplementRule>();

			rule.ForTypes.Should().BeOfType<GenericMatcher>()
				.Which.Type.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("GenericType");

			rule.ForTypes.Should().BeOfType<GenericMatcher>()
				.Which.TypeArguments.Should().HaveCount(1)
				.And.ContainSingle(m => m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "System.String");

			rule.Interface.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustNotImplementInterface");
		}

		rules[3].Should().BeOfType<MustNotInheritRule>();
		{
			var rule = rules[3].As<MustNotInheritRule>();

			rule.ForTypes.Should().BeOfType<NotMatcher>()
				.Which.Matcher.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustNotInheritName");

			rule.BaseType.Should().BeOfType<OrMatcher>()
				.Which.Matchers.Should().HaveCount(2)
				.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "MustNotInheritBaseType")
				.And.ContainSingle(m =>
					m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "MustNotInheritFullBaseType");
		}
	}
}