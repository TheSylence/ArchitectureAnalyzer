using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustNotImplementRule : Rule
{
	public Matcher Interface { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor => Diagnostics.MustNotImplement;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol)
	{
		if (!symbol.AllInterfaces.Any(i => Interface.Matches(i)))
			return null;

		return CreateDiagnostic(symbol, symbol.Name, Interface.ToString());
	}
}