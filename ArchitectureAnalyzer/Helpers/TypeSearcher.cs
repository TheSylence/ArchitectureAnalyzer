using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Helpers;

internal static class TypeSearcher
{
	public static IEnumerable<INamedTypeSymbol> FindAllSymbols(this Compilation compilation)
	{
		return compilation.References
			.Select(compilation.GetAssemblyOrModuleSymbol)
			.OfType<IAssemblySymbol>()
			.SelectMany(GetNamespaces)
			.SelectMany(n => n.GetTypeMembers())
			.Concat(GetNamespaces(compilation.Assembly.GlobalNamespace).SelectMany(a => a.GetTypeMembers()))
			.Distinct(SymbolEqualityComparer.Default)
			.Where(t => t?.CanBeReferencedByName == true)
			.OfType<INamedTypeSymbol>()
			.SelectMany(t => FindNestedTypes(t).Append(t));
	}

	private static IEnumerable<INamedTypeSymbol> FindNestedTypes(INamedTypeSymbol type)
	{
		return type.GetTypeMembers().SelectMany(t => FindNestedTypes(t).Append(t));
	}

	private static IEnumerable<INamespaceSymbol> GetNamespaces(IAssemblySymbol asm)
	{
		foreach (var ns in asm.GlobalNamespace.ConstituentNamespaces)
		{
			foreach (var namespaceMember in ns.GetNamespaceMembers())
			{
				foreach (var member in GetNamespaces(namespaceMember))
				{
					yield return member;
				}
			}
		}
	}

	private static IEnumerable<INamespaceSymbol> GetNamespaces(INamespaceSymbol ns)
	{
		yield return ns;
		foreach (var namespaceMember in ns.GetNamespaceMembers())
		{
			foreach (var member in GetNamespaces(namespaceMember))
			{
				yield return member;
			}
		}
	}
}