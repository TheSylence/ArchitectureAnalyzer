using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.Helper;

internal sealed class SymbolBuilder
{
	public SymbolBuilder()
	{
		_symbol = Substitute.For<INamedTypeSymbol>();

		_symbol.Locations.Returns(new[] { Location.None }.ToImmutableArray());
		_symbol.BaseType.Returns((INamedTypeSymbol?)null);
	}

	public INamedTypeSymbol Build() => _symbol;
	
	public SymbolBuilder WithSpecialType(SpecialType specialType)
	{
		_symbol.SpecialType.Returns(specialType);
		return this;
	}

	public SymbolBuilder WithAttribute(string name)
	{
		var attributeSymbol =
			new SymbolBuilder().WithName(name).WithBaseType("System.Attribute").Build();
		var attributeData = new MockAttributeData(attributeSymbol);

		_symbol.GetAttributes().Returns(new AttributeData[] { attributeData }.ToImmutableArray());

		return this;
	}

	public SymbolBuilder WithBaseType(INamedTypeSymbol baseType)
	{
		_symbol.BaseType.Returns(baseType);
		return this;
	}

	public SymbolBuilder WithBaseType(string baseType)
	{
		_symbol.BaseType.Returns(_ => new SymbolBuilder().WithName(baseType).Build());
		return this;
	}

	public SymbolBuilder WithGenericArguments(params INamedTypeSymbol[] genericArguments)
	{
		_symbol.TypeArguments.Returns(genericArguments.Cast<ITypeSymbol>().ToImmutableArray());
		_symbol.IsGenericType.Returns(true);
		return this;
	}

	public SymbolBuilder WithInterfaces(params INamedTypeSymbol[] interfaces)
	{
		_symbol.AllInterfaces.Returns(interfaces.ToImmutableArray());
		return this;
	}

	public SymbolBuilder WithInterfaces(params string[] interfaces)
	{
		_symbol.AllInterfaces.Returns(_ =>
			interfaces.Select(i => new SymbolBuilder().WithName(i).Build()).ToImmutableArray());
		return this;
	}

	public SymbolBuilder WithName(string name)
	{
		_symbol.Name.Returns(name);
		return this;
	}

	public SymbolBuilder WithNamespace(string @namespace)
	{
		_symbol.ContainingNamespace.Returns(
			_ => new NamespaceBuilder().WithName(@namespace).Build());
		return this;
	}

	public SymbolBuilder WithNoAttributes()
	{
		_symbol.GetAttributes().Returns(ImmutableArray<AttributeData>.Empty);
		return this;
	}

	public SymbolBuilder WithNoBaseType()
	{
		_symbol.BaseType.Returns((INamedTypeSymbol?)null);
		return this;
	}

	public SymbolBuilder WithNoInterfaces()
	{
		_symbol.AllInterfaces.Returns(ImmutableArray<INamedTypeSymbol>.Empty);
		return this;
	}

	private readonly INamedTypeSymbol _symbol;
}