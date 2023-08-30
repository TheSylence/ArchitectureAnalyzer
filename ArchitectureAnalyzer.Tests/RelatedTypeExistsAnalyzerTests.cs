using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis.CSharp.Testing;

namespace ArchitectureAnalyzer.Tests;

public sealed class RelatedTypeExistsAnalyzerTests
{
	[Fact]
	public async Task IsNotTriggered_WhenUsingGenericTypes()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class TestCommand{}", "public class TestCommandHandler : ICommandHandler<TestCommand> {} public interface ICommandHandler<T>{}" },
				AdditionalFiles =
				{
					("architecture.rules.json", @"
						{
						""rules"": [
							{
								""relatedTypeExists"": {
								""relatedType"": { ""implements"": { ""generic"": { ""type"": { ""name"": ""ICommandHandler"" }, ""typeArguments"": [ { ""fullName"": ""%type.FullName%"" } ] } } },
								""forTypes"": { ""name"": ""*Command"" }
							}}
						]
						}")
				}
			}
		};

		await verifier.RunAsync();
		
	}
	
	[Fact]
	public async Task IsNotTriggered_WhenForbidden()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class TestCommand{}", "public class TestCommandOther {}" },
				AdditionalFiles =
				{
					("architecture.rules.json", @"
						{
						""rules"": [
							{
								""relatedTypeExists"": {
								""forbidden"": true,
								""relatedType"": { ""name"": ""%type.Name%Handler"" },
								""forTypes"": { ""name"": ""*Command"" }
							}}
						]
						}")
				}
			}
		};

		await verifier.RunAsync();
	}

	[Fact]
	public async Task IsNotTriggered_WhenValid()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class TestCommandHandler {}", "public class TestCommand{}" },
				AdditionalFiles =
				{
					("architecture.rules.json", @"
						{
						""rules"": [
							{
								""relatedTypeExists"": {
								""relatedType"": { ""name"": ""%type.Name%Handler"" },
								""forTypes"": { ""name"": ""*Command"" }
							}}
						]
						}")
				}
			}
		};

		await verifier.RunAsync();
	}

	[Fact]
	public async Task IsTriggered_WhenForbidden()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class TestCommand{}", "public class TestCommandHandler {}" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0006").WithLocation(1, 14)
						.WithArguments("TestCommand", "Name: TestCommandHandler")
				},
				AdditionalFiles =
				{
					("architecture.rules.json", @"
						{
						""rules"": [
							{
								""relatedTypeExists"": {
								""forbidden"": true,
								""relatedType"": { ""name"": ""%type.Name%Handler"" },
								""forTypes"": { ""name"": ""*Command"" }
							}}
						]
						}")
				}
			}
		};

		await verifier.RunAsync();
	}

	[Fact]
	public async Task IsTriggered_WhenInvalid()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class TestCommand{}", "public class TestCommandOther {}" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0005").WithLocation(1, 14)
						.WithArguments("TestCommand", "Name: TestCommandHandler")
				},
				AdditionalFiles =
				{
					("architecture.rules.json", @"
						{
						""rules"": [
							{
								""relatedTypeExists"": {
								""relatedType"": { ""name"": ""%type.Name%Handler"" },
								""forTypes"": { ""name"": ""*Command"" }
							}}
						]
						}")
				}
			}
		};

		await verifier.RunAsync();
	}
}