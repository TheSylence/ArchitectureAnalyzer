namespace ArchitectureAnalyzer.Tests;

public sealed class ArchitectureAnalyzerTests
{
	[Fact]
	public async Task DoesNothing_WhenNoJsonFileIsFound()
	{
		await Verify.VerifyAnalyzerAsync("public class Foo {}");
	}
}