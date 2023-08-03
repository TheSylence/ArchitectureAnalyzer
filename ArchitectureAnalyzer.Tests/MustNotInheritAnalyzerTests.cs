using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis.CSharp.Testing;

namespace ArchitectureAnalyzer.Tests;

public sealed class MustNotInheritAnalyzerTests
{
	[Fact]
	public async Task IsNotTriggered_WhenValid()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo {}" },
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustNotInherit"": {
								""baseType"": { ""fullName"": ""System.Attribute"" },
								""forTypes"": { ""name"": ""*"" }
							}}
						]
						}"
					)
				}
			}
		};

		await verifier.RunAsync();
	}

	[Fact]
	public async Task IsTriggered_WhenViolated()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo : System.Attribute {}" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0004").WithArguments("Foo", "FullName: System.Attribute").WithLocation(1, 14)
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustNotInherit"": {
								""baseType"": { ""fullName"": ""System.Attribute"" },
								""forTypes"": { ""name"": ""*"" }
							}}
						]
						}"
					)
				}
			}
		};

		await verifier.RunAsync();
	}
}