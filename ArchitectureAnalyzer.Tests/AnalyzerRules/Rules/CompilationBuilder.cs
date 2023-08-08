using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules.Rules;

internal static class CompilationBuilder
{
	public static Compilation Compile(string source)
	{
		var tree = CSharpSyntaxTree.ParseText(source);

		return CSharpCompilation.Create("test").AddReferences(
				MetadataReference.CreateFromFile(
					typeof(object).Assembly.Location))
			.AddSyntaxTrees(tree);
	}
}