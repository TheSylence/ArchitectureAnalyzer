using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis.CSharp.Testing;

namespace ArchitectureAnalyzer.Tests;

public sealed class ArchitectureAnalyzerTests
{
	[Fact]
	public async Task DoesNothing_WhenNoCodeIsPresent()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "" },
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""interface"": { ""fullName"": ""System.IDisposable"" },
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
	public async Task DoesNothing_WhenNoJsonFileIsFound()
	{
		await Verify.VerifyAnalyzerAsync("public class Foo {}");
	}

	[Fact]
	public async Task DoesNothing_WhenNoRulesAreDefined()
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
						""rules"": []
						}"
					)
				}
			}
		};

		await verifier.RunAsync();
	}
}