using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class RelatedTypeExistsRule : Rule
{
	public Matcher RelatedType { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor =>
		Forbidden ? Diagnostics.RelatedTypeMustNotExist : Diagnostics.RelatedTypeExists;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation)
	{
		var relatedTypeMatcher = MatchReplacer.Replace(RelatedType, symbol);
		
		var symbols = compilation.FindAllSymbols();

		var exists = symbols.Any(s => relatedTypeMatcher.Matches(s));

		if ((exists && !Forbidden) || (!exists && Forbidden))
			return null;

		return CreateDiagnostic(symbol, symbol.Name, relatedTypeMatcher.ToString());
	}
}