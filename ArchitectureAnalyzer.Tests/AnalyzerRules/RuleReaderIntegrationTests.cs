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
      "mustImplement": {
        "forbidden": true,
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
      "mustInherit": {
        "forbidden": true,
        "forTypes": { "not": { "name": "MustNotInheritName" } },
        "baseType": {
          "or": [
            { "name": "MustNotInheritBaseType" },
            { "fullName": "MustNotInheritFullBaseType" }
          ]
        }
      }
    },
    {
      "relatedTypeExists": {
        "relatedType": {
          "implements": {
            "generic": {
              "type": { "name": "IValidator" },
              "typeArguments": [{ "fullName": "%type.FullName%" }]
            }
          }
        },
        "forTypes": {
          "name": "*Request"
        }
      }
    },
	{
		"mustReference": {
			"forTypes": { "namespace": "*.Tests" },
			"reference": { "name": "*Service" }
		}
	}
  ]
}
""";

		// Act
		var rules = _sut.Read(json).ToList();

		// Assert
		using var _ = new AssertionScope();

		rules.Should().HaveCount(6);

		rules[0].Should().BeOfType<MustImplementRule>();
		{
			var rule = rules[0].As<MustImplementRule>();

			rule.Forbidden.Should().BeFalse();

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

			rule.Forbidden.Should().BeFalse();

			rule.ForTypes.Should().BeOfType<FullNameMatcher>()
				.Which.FullName.Should().Be("MustInheritFullName");

			rule.BaseType.Should().BeOfType<FullNameMatcher>()
				.Which.FullName.Should().Be("MustInheritBaseType");
		}

		rules[2].Should().BeOfType<MustImplementRule>();
		{
			var rule = rules[2].As<MustImplementRule>();

			rule.Forbidden.Should().BeTrue();

			rule.ForTypes.Should().BeOfType<GenericMatcher>()
				.Which.Type.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("GenericType");

			rule.ForTypes.Should().BeOfType<GenericMatcher>()
				.Which.TypeArguments.Should().HaveCount(1)
				.And.ContainSingle(m => m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "System.String");

			rule.Interface.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustNotImplementInterface");
		}

		rules[3].Should().BeOfType<MustInheritRule>();
		{
			var rule = rules[3].As<MustInheritRule>();

			rule.Forbidden.Should().BeTrue();

			rule.ForTypes.Should().BeOfType<NotMatcher>()
				.Which.Matcher.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustNotInheritName");

			rule.BaseType.Should().BeOfType<OrMatcher>()
				.Which.Matchers.Should().HaveCount(2)
				.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "MustNotInheritBaseType")
				.And.ContainSingle(m =>
					m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "MustNotInheritFullBaseType");
		}

		rules[4].Should().BeOfType<RelatedTypeExistsRule>();
		{
			var rule = rules[4].As<RelatedTypeExistsRule>();

			rule.ForTypes.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("*Request");

			rule.RelatedType.Should().BeOfType<ImplementsMatcher>()
				.Which.Type.Should().BeOfType<GenericMatcher>()
				.Which.Type.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("IValidator");

			rule.RelatedType.Should().BeOfType<ImplementsMatcher>()
				.Which.Type.Should().BeOfType<GenericMatcher>()
				.Which.TypeArguments.Should().HaveCount(1).And.ContainSingle(x =>
					x is FullNameMatcher && x.As<FullNameMatcher>().FullName == "%type.FullName%");
		}

		rules[5].Should().BeOfType<MustReferenceRule>();
		{
			var rule = rules[5].As<MustReferenceRule>();

			rule.ForTypes.Should().BeOfType<NamespaceMatcher>().Which.Namespace.Should().Be("*.Tests");
			
			rule.Reference.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("*Service");
		}
	}
}