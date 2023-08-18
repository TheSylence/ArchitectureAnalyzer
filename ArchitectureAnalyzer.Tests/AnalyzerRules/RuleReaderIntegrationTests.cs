using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using FluentAssertions.Execution;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules;

public sealed class RuleReaderIntegrationTests
{
	private const string Json = """
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
	                            	},
	                              {
	                                "mustBeInNamespace": {
	                                  "forTypes": { "namespace": "*.Tests" },
	                                  "namespace": "MyCompany.MyProject.Tests"
	                                }
	                              }
	                              ]
	                            }
	                            """;

	private readonly RuleReader _sut = new(new MatchReader());

	private static void Rule6(Rule rule)
	{
		rule.Should().BeOfType<MustBeInNamespaceRule>();
		var r = rule.As<MustBeInNamespaceRule>();

		r.ForTypes.Should().BeOfType<NamespaceMatcher>().Which.Namespace.Should().Be("*.Tests");
		r.Namespace.Should().Be("MyCompany.MyProject.Tests");
	}

	private static void Rule5(Rule rule)
	{
		rule.Should().BeOfType<MustReferenceRule>();
		var r = rule.As<MustReferenceRule>();

		r.ForTypes.Should().BeOfType<NamespaceMatcher>().Which.Namespace.Should().Be("*.Tests");

		r.Reference.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("*Service");
	}

	private static void Rule4(Rule rule)
	{
		rule.Should().BeOfType<RelatedTypeExistsRule>();
		var r = rule.As<RelatedTypeExistsRule>();

		r.ForTypes.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("*Request");

		r.RelatedType.Should().BeOfType<ImplementsMatcher>()
			.Which.Type.Should().BeOfType<GenericMatcher>()
			.Which.Type.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("IValidator");

		r.RelatedType.Should().BeOfType<ImplementsMatcher>()
			.Which.Type.Should().BeOfType<GenericMatcher>()
			.Which.TypeArguments.Should().HaveCount(1).And.ContainSingle(x =>
				x is FullNameMatcher && x.As<FullNameMatcher>().FullName == "%type.FullName%");
	}

	private static void Rule3(Rule rule)
	{
		rule.Should().BeOfType<MustInheritRule>();
		{
			var r = rule.As<MustInheritRule>();

			r.Forbidden.Should().BeTrue();

			r.ForTypes.Should().BeOfType<NotMatcher>()
				.Which.Matcher.Should().BeOfType<NameMatcher>()
				.Which.Name.Should().Be("MustNotInheritName");

			r.BaseType.Should().BeOfType<OrMatcher>()
				.Which.Matchers.Should().HaveCount(2)
				.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "MustNotInheritBaseType")
				.And.ContainSingle(m =>
					m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "MustNotInheritFullBaseType");
		}
	}

	private static void Rule2(Rule rule)
	{
		rule.Should().BeOfType<MustImplementRule>();
		var r = rule.As<MustImplementRule>();

		r.Forbidden.Should().BeTrue();

		r.ForTypes.Should().BeOfType<GenericMatcher>()
			.Which.Type.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("GenericType");

		r.ForTypes.Should().BeOfType<GenericMatcher>()
			.Which.TypeArguments.Should().HaveCount(1)
			.And.ContainSingle(m => m is FullNameMatcher && m.As<FullNameMatcher>().FullName == "System.String");

		r.Interface.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("MustNotImplementInterface");
	}

	private static void Rule1(Rule rule)
	{
		rule.Should().BeOfType<MustInheritRule>();
		var r = rule.As<MustInheritRule>();

		r.Forbidden.Should().BeFalse();

		r.ForTypes.Should().BeOfType<FullNameMatcher>()
			.Which.FullName.Should().Be("MustInheritFullName");

		r.BaseType.Should().BeOfType<FullNameMatcher>()
			.Which.FullName.Should().Be("MustInheritBaseType");
	}

	private static void Rule0(Rule rule)
	{
		rule.Should().BeOfType<MustImplementRule>();
		var r = rule.As<MustImplementRule>();

		r.Forbidden.Should().BeFalse();

		r.ForTypes.Should().BeOfType<AndMatcher>()
			.Which.Matchers.Should().HaveCount(2)
			.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "*MustImplementName")
			.And.ContainSingle(m => m is NameMatcher && m.As<NameMatcher>().Name == "Start*");

		r.Interface.Should().BeOfType<NameMatcher>()
			.Which.Name.Should().Be("MustImplementInterface");
	}

	[Fact]
	public void Reads_CompleteRuleSet()
	{
		// Arrange

		// Act
		var rules = _sut.Read(Json).ToList();

		// Assert
		using var _ = new AssertionScope();

		rules.Should().HaveCount(7)
			.And.SatisfyRespectively(
				Rule0, Rule1, Rule2, Rule3, Rule4, Rule5, Rule6);
	}
}