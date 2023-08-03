using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustNotInheritRule : Rule
{
	public Matcher BaseType { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor => Diagnostics.MustNotInherit;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol)
	{
		if (symbol.BaseType is null)
			return null;

		if (!BaseType.Matches(symbol.BaseType))
			return null;

		return CreateDiagnostic(symbol, symbol.Name, BaseType.ToString());
	}
}