using ArchitectureAnalyzer.Tests.Helper;
using Microsoft.CodeAnalysis.CSharp.Testing;

namespace ArchitectureAnalyzer.Tests;

public sealed class MustImplementAnalyzerTests
{
	[Fact]
	public async Task IsNotTriggered_WhenForbidden_WhenValid()
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
								""mustImplement"": {
								""forbidden"": true,
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
	public async Task IsTriggered_WhenForbidden_WhenViolated()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo : System.IDisposable { public void Dispose() {} }" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0003").WithLocation(1, 14)
						.WithArguments("Foo", "FullName: System.IDisposable")
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""forbidden"": true,
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
	public async Task IsTriggered_WhenForbidden_WhenViolated_WhenImplementingGeneric()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources = { "public class Foo : System.IEquatable<Foo> { public bool Equals(Foo other) => false; }" },
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0003").WithLocation(1, 14)
						.WithArguments("Foo", "FullName: System.IEquatable<*>")
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""forbidden"": true,
								""interface"": { ""fullName"": ""System.IEquatable<*>"" },
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
	public async Task IsTriggered_WhenForbidden_WhenViolated_WithMultipleInterfaces()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources =
				{
					"public class Foo : System.IDisposable, System.IEquatable<Foo> { public void Dispose() {} public bool Equals(Foo other) => false; }"
				},
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0003").WithLocation(1, 14)
						.WithArguments("Foo", "FullName: System.IDisposable")
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""forbidden"": true,
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
	public async Task IsTriggered_WhenForbidden_WhenViolated_WithMultipleInterfaces_WithDifferentRules()
	{
		var verifier = new CSharpAnalyzerTest<ArchitectureAnalyzer, FluentVerifier>
		{
			TestState =
			{
				Sources =
				{
					"public class Foo : System.IDisposable, System.IComparable { public void Dispose() {} public int CompareTo(object? other) => 0; }"
				},
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("AA0003").WithLocation(1, 14)
						.WithArguments("Foo", "FullName: System.IDisposable"),
					Verify.Diagnostic("AA0003").WithLocation(1, 14)
						.WithArguments("Foo", "FullName: System.IComparable")
				},
				AdditionalFiles =
				{
					(
						"architecture.rules.json", @"
						{
						""rules"": [
							{
								""mustImplement"": {
								""forbidden"": true,
								""interface"": { ""fullName"": ""System.IDisposable"" },
								""forTypes"": { ""name"": ""*"" }
							}},
							{
								""mustImplement"": {
								""forbidden"": true,
								""interface"": { ""fullName"": ""System.IComparable"" },
								""forTypes"": { ""name"": ""Foo"" }
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