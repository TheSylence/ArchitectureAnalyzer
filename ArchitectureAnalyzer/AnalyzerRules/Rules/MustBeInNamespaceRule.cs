using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustBeInNamespaceRule : Rule
{
	public string Namespace { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor =>
		Forbidden ? Diagnostics.MustNotBeInNamespace : Diagnostics.MustBeInNamespace;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation)
	{
		var symbolNamespace = symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		var wildcard = Namespace.Prefix("global::");
		var matches = wildcard.Matches(symbolNamespace);

		if ((Forbidden && matches) || (!Forbidden && !matches))
			return CreateDiagnostic(symbol, symbolNamespace, Namespace);

		return null;
	}
}