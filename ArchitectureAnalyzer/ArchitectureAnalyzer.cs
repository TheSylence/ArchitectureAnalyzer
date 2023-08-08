using System.Collections.Immutable;
using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ArchitectureAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ArchitectureAnalyzer : DiagnosticAnalyzer
{
	public ArchitectureAnalyzer()
	{
		var matchReader = new MatchReader();
		_ruleReader = new RuleReader(matchReader);
	}

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.All().ToImmutableArray();

	public override void Initialize(AnalysisContext context)
	{
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

		context.RegisterCompilationStartAction(OnCompilationStart);
	}

	private IEnumerable<Rule> FindRulesIn(IEnumerable<AdditionalText> files,
		CancellationToken cancellationToken)
	{
		var result = new List<Rule>();

		foreach (var additionalFile in files)
		{
			if (Path.GetFileName(additionalFile.Path) != "architecture.rules.json")
				continue;

			var text = additionalFile.GetText(cancellationToken);
			if (text is null)
				continue;

			var json = text.ToString();
			var rules = _ruleReader.Read(json);

			result.AddRange(rules);
		}

		return result;
	}

	private void OnCompilationStart(CompilationStartAnalysisContext context)
	{
		var rules = FindRulesIn(context.Options.AdditionalFiles, context.CancellationToken).ToList();

		foreach (var rule in (IEnumerable<Rule>)rules)
		{
			context.RegisterSymbolAction(
				symbolAnalysisContext =>
				{
					if (symbolAnalysisContext.Symbol is not INamedTypeSymbol symbol)
						return;

					var diagnostic = rule.Evaluate(symbol, symbolAnalysisContext.Compilation);
					if (diagnostic is null)
						return;

					symbolAnalysisContext.ReportDiagnostic(diagnostic);
				},
				SymbolKindsForRules);
		}
	}

	private readonly RuleReader _ruleReader;

	private static readonly ImmutableArray<SymbolKind>
		SymbolKindsForRules = ImmutableArray.Create(SymbolKind.NamedType);
}