using Microsoft.CodeAnalysis;

namespace ArchitectureAnalyzer.Tests.Helper;

internal sealed class NamespaceBuilder
{
	public INamespaceSymbol Build() => _symbol;

	public NamespaceBuilder WithName(string name)
	{
		_symbol.Name.Returns(name);
		return this;
	}

	private readonly INamespaceSymbol _symbol = Substitute.For<INamespaceSymbol>();
}