using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustInheritRule : Rule
{
	public Matcher BaseType { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor =>
		Forbidden ? Diagnostics.MustNotInherit : Diagnostics.MustInherit;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation)
	{
		if (Forbidden)
		{
			if (symbol.BaseType is null)
				return null;

			if (BaseType.Matches(symbol.BaseType))
				return CreateDiagnostic(symbol, symbol.Name, BaseType.ToString());
		}
		else
		{
			if (symbol.BaseType is null)
				return CreateDiagnostic(symbol, symbol.Name, BaseType.ToString());

			if (!BaseType.Matches(symbol.BaseType))
				return CreateDiagnostic(symbol, symbol.Name, BaseType.ToString());
		}

		return null;
	}
}