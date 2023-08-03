using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis.CSharp.Testing;

namespace ArchitectureAnalyzer.Tests;

public sealed class MustImplementAnalyzerTests
{
	[Fact]
	public async Task IsNotTriggered_WhenValid()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo : System.IDisposable { public void Dispose() {} }" },
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
	public async Task IsTriggered_WhenViolated()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo {}" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0001").WithLocation(1, 14).WithArguments("Foo", "Name: System.IDisposable")
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""interface"": { ""name"": ""System.IDisposable"" },
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