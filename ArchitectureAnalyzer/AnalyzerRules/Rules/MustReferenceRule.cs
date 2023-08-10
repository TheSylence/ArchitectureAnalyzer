using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using ArchitectureAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ArchitectureAnalyzer.AnalyzerRules.Rules;

internal sealed class MustReferenceRule : Rule
{
	public Matcher Reference { get; set; } = default!;

	protected override DiagnosticDescriptor Descriptor =>
		Forbidden ? Diagnostics.MustNotReference : Diagnostics.MustReference;

	protected override Diagnostic? EvaluateInternal(INamedTypeSymbol symbol, Compilation compilation)
	{
		var references = GetReferencedTypes(symbol, compilation).WhereNotNull();

		var referenceMatcher = MatchReplacer.Replace(Reference, symbol);

		var matchingReferences = references.Where(r => referenceMatcher.Matches(r)).ToList();

		if ((Forbidden && matchingReferences.Any()) || (!Forbidden && !matchingReferences.Any()))
			return CreateDiagnostic(symbol, symbol.Name, Reference.ToString());

		return null;
	}

	private static IEnumerable<INamedTypeSymbol?> GetReferencedTypes(INamedTypeSymbol symbol, Compilation compilation)
	{
		var members = symbol.GetMembers();

		var fields = members.Where(m => m.Kind == SymbolKind.Field).Cast<IFieldSymbol>();
		foreach (var field in fields)
		{
			yield return field.Type as INamedTypeSymbol;
		}

		var methods = members.Where(m => m.Kind == SymbolKind.Method).Cast<IMethodSymbol?>().ToList();

		var properties = members.Where(m => m.Kind == SymbolKind.Property).Cast<IPropertySymbol>();
		foreach (var property in properties)
		{
			yield return property.Type as INamedTypeSymbol;

			methods.Add(property.SetMethod);
			methods.Add(property.GetMethod);
		}

		foreach (var method in methods.WhereNotNull())
		{
			var methodReferences = GetReferencedTypes(method, compilation);
			foreach (var reference in methodReferences)
			{
				yield return reference;
			}
		}

		foreach (var typeArgument in symbol.TypeArguments)
		{
			yield return typeArgument as INamedTypeSymbol;
		}
	}

	private static IEnumerable<INamedTypeSymbol?> GetReferencedTypes(IMethodSymbol method,
		Compilation compilation)
	{
		yield return method.ReturnType as INamedTypeSymbol;

		foreach (var parameter in method.Parameters)
		{
			yield return parameter.Type as INamedTypeSymbol;
		}

		foreach (var typeArgument in method.TypeArguments)
		{
			yield return typeArgument as INamedTypeSymbol;
		}

		foreach (var syntax in method.DeclaringSyntaxReferences)
		{
			foreach (var type in GetReferencedTypes(syntax.GetSyntax(), compilation))
			{
				yield return type;
			}
		}
	}

	private static IEnumerable<INamedTypeSymbol> GetReferencedTypes(SyntaxNode node, Compilation compilation)
	{
		if (node is not MethodDeclarationSyntax methodDeclaration)
			yield break;

		if (methodDeclaration.Body is null)
			yield break;

		var semanticModel = compilation.GetSemanticModel(node.SyntaxTree);

		
		foreach (var declarationSyntax in methodDeclaration.Body.Statements.OfType<LocalDeclarationStatementSyntax>())
		{
			var typeInfo = semanticModel.GetSymbolInfo(declarationSyntax.Declaration.Type);

			if (typeInfo.Symbol is INamedTypeSymbol namedTypeSymbol)
				yield return namedTypeSymbol;
		}
	}
}