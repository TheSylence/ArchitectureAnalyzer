using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal abstract class Rule
{
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public string Description { get; set; } = default!;
	public bool Forbidden { get; set; }
	public Matcher ForTypes { get; set; } = default!;

	protected abstract DiagnosticDescriptor Descriptor { get; }

	public Diagnostic? Evaluate(INamedTypeSymbol symbol, Compilation compilation)
	{
		if (!ForTypes.Matches(symbol))
			return null;

		return EvaluateInternal(symbol, compilation);
	}

	protected Diagnostic CreateDiagnostic(INamedTypeSymbol symbol, params string[] messageArgs)
	{
		var location = symbol.Locations.First();
		var additionalLocations = symbol.Locations.Skip(1);

		// ReSharper disable once CoVariantArrayConversion
		return Diagnostic.Create(Descriptor, location, additionalLocations, messageArgs);
	}

	protected abstract Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation);
}