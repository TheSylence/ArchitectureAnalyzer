﻿using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

internal sealed class IsMatcher : Matcher
{
	public IsKind Kind { get; set; }

	public override bool Matches(INamedTypeSymbol symbol)
	{
		if (!MatchAccessibility(symbol))
			return false;

		if (!MatchTypeKind(symbol))
			return false;

		if (!MatchModifier(symbol))
			return false;

		return true;
	}

	public override string ToString()
	{
		var parts = new List<string>();

		AddAccessibilityDisplayParts(parts);
		AddModifierDisplayParts(parts);
		AddTypeKindDisplayParts(parts);

		return "Is: " + string.Join(" ", parts);
	}

	private void AddAccessibilityDisplayParts(List<string> parts)
	{
		if (Kind.HasFlag(IsKind.Public))
			parts.Add("public");
		else if (Kind.HasFlag(IsKind.Private))
			parts.Add("private");
		else if (Kind.HasFlag(IsKind.Protected))
			parts.Add("protected");
		else if (Kind.HasFlag(IsKind.Internal))
			parts.Add("internal");
	}

	private void AddModifierDisplayParts(List<string> parts)
	{
		if (Kind.HasFlag(IsKind.Abstract))
			parts.Add("abstract");
		else if (Kind.HasFlag(IsKind.Sealed))
			parts.Add("sealed");
		else if (Kind.HasFlag(IsKind.Static))
			parts.Add("static");
	}

	private void AddTypeKindDisplayParts(List<string> parts)
	{
		if (Kind.HasFlag(IsKind.Class))
			parts.Add("class");
		else if (Kind.HasFlag(IsKind.Interface))
			parts.Add("interface");
		else if (Kind.HasFlag(IsKind.Struct))
			parts.Add("struct");
		else if (Kind.HasFlag(IsKind.Enum))
			parts.Add("enum");
	}

	private bool MatchAccessibility(INamedTypeSymbol symbol)
	{
		var accessibility = symbol.DeclaredAccessibility;

		if (Kind.HasFlag(IsKind.Public) && accessibility != Accessibility.Public)
			return false;

		if (Kind.HasFlag(IsKind.Protected) && accessibility != Accessibility.Protected)
			return false;

		if (Kind.HasFlag(IsKind.Private) && accessibility != Accessibility.Private)
			return false;

		if (Kind.HasFlag(IsKind.Internal) && accessibility != Accessibility.Internal)
			return false;

		return true;
	}

	private bool MatchModifier(INamedTypeSymbol symbol)
	{
		if (Kind.HasFlag(IsKind.Abstract) && !symbol.IsAbstract)
			return false;

		if (Kind.HasFlag(IsKind.Sealed) && !symbol.IsSealed)
			return false;

		if (Kind.HasFlag(IsKind.Static) && !symbol.IsStatic)
			return false;

		return true;
	}

	private bool MatchTypeKind(INamedTypeSymbol symbol)
	{
		var typeKind = symbol.TypeKind;

		if (Kind.HasFlag(IsKind.Class) && typeKind != TypeKind.Class)
			return false;

		if (Kind.HasFlag(IsKind.Interface) && typeKind != TypeKind.Interface)
			return false;

		if (Kind.HasFlag(IsKind.Struct) && typeKind != TypeKind.Struct)
			return false;

		if (Kind.HasFlag(IsKind.Enum) && typeKind != TypeKind.Enum)
			return false;

		return true;
	}
}