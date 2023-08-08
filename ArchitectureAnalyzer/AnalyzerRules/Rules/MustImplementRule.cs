using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustImplementRule : Rule
{
	public Matcher Interface { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor =>
		Forbidden ? Diagnostics.MustNotImplement : Diagnostics.MustImplement;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation)
	{
		var implements = symbol.AllInterfaces.Any(i => Interface.Matches(i));
        
		if (implements && !Forbidden || !implements && Forbidden)
			return null;

		return CreateDiagnostic(symbol, symbol.Name, Interface.ToString());
	}
}