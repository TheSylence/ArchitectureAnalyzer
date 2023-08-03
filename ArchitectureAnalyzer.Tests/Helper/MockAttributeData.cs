using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.Helper;

internal sealed class MockAttributeData : AttributeData
{
	public MockAttributeData(INamedTypeSymbol symbol)
	{
		CommonAttributeClass = symbol;
	}

	protected override SyntaxReference? CommonApplicationSyntaxReference { get; } = null;

	protected override INamedTypeSymbol? CommonAttributeClass { get; }
	protected override IMethodSymbol? CommonAttributeConstructor { get; } = null;

	protected override ImmutableArray<TypedConstant> CommonConstructorArguments =>
		ImmutableArray<TypedConstant>.Empty;

	protected override ImmutableArray<KeyValuePair<string, TypedConstant>> CommonNamedArguments =>
		ImmutableArray<KeyValuePair<string, TypedConstant>>.Empty;
}