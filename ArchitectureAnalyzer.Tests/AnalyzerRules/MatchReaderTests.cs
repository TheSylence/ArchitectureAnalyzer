using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Matchers;
using LightJson;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules;

public sealed class MatchReaderTests
{
	private readonly MatchReader _sut = new();

	[Fact]
	public void Reads_AndMatcher()
	{
		// Arrange
		const string json =
			"""{ "and": [{"name": "InterfaceName"},{"name":"otherName"}] }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<AndMatcher>().Which.Matchers.Should()
			.ContainSingle(m => (m as NameMatcher)!.Name == "InterfaceName")
			.And.ContainSingle(m => (m as NameMatcher)!.Name == "otherName");
	}

	[Fact]
	public void Reads_FullNameMatcher()
	{
		// Arrange
		const string json =
			"""{ "fullName": "Namespace.Type" }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<FullNameMatcher>().Which.FullName.Should().Be("Namespace.Type");
	}

	[Fact]
	public void Reads_GenericMatcher()
	{
		// Arrange
		const string json =
			"""{ "generic": { "type": {"name": "InterfaceName"}, "typeArguments": [ {"name": "GenericName"} ] } }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<GenericMatcher>();
		var genericMatcher = (result as GenericMatcher)!;
		genericMatcher.Type.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("InterfaceName");
		genericMatcher.TypeArguments.Should().HaveCount(1).And
			.ContainSingle(x => x is NameMatcher && (x as NameMatcher)!.Name == "GenericName");
	}

	[Fact]
	public void Reads_ImplementsMatcher()
	{
		// Arrange
		const string json =
			"""{ "implements": { "type": {"name": "InterfaceName"} } }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<ImplementsMatcher>().Which.Type.Should()
			.BeOfType<NameMatcher>().Which.Name.Should().Be("InterfaceName");
	}

	[Fact]
	public void Reads_InheritsMatcher()
	{
		// Arrange
		const string json =
			"""{ "inherits": { "type": {"name": "InterfaceName"} } }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<InheritsMatcher>().Which.Type.Should()
			.BeOfType<NameMatcher>().Which.Name.Should().Be("InterfaceName");
	}

	[Fact]
	public void Reads_NameMatcher()
	{
		// Arrange
		const string json =
			"""{ "name": "InterfaceName" }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<NameMatcher>().Which.Name.Should().Be("InterfaceName");
	}

	[Fact]
	public void Reads_NotMatcher()
	{
		// Arrange
		const string json =
			"""{ "not": {"name": "InterfaceName"} }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<NotMatcher>().Which.Matcher.Should()
			.BeOfType<NameMatcher>().Which.Name.Should().Be("InterfaceName");
	}

	[Theory]
	[InlineData("class", IsKind.Class)]
	[InlineData("interface", IsKind.Interface)]
	[InlineData("struct", IsKind.Struct)]
	[InlineData("enum", IsKind.Enum)]
	[InlineData("static", IsKind.Static)]
	[InlineData("abstract", IsKind.Abstract)]
	[InlineData("sealed", IsKind.Sealed)]
	[InlineData("public", IsKind.Public)]
	[InlineData("internal", IsKind.Internal)]
	[InlineData("private", IsKind.Private)]
	[InlineData("protected", IsKind.Protected)]
	[InlineData("class,static", IsKind.Class | IsKind.Static)]
	[InlineData("interface,public", IsKind.Interface | IsKind.Public)]
	[InlineData("class,static,public", IsKind.Class | IsKind.Static | IsKind.Public)]
	public void Reads_IsMatcher(string kindString, IsKind expectedKind)
	{
		// Arrange
		var json = $"{{ \"is\": \"{kindString}\" }}";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<IsMatcher>().Which.Kind.Should().Be(expectedKind);
	}

	[Fact]
	public void Reads_OrMatcher()
	{
		// Arrange
		const string json =
			"""{ "or": [{"name": "InterfaceName"},{"name":"otherName"}] }""";

		var jsonObject = JsonValue.Parse(json).AsJsonObject!;

		// Act
		var result = _sut.ReadMatcher(jsonObject);

		// Assert
		result.Should().BeOfType<OrMatcher>().Which.Matchers.Should()
			.ContainSingle(m => (m as NameMatcher)!.Name == "InterfaceName")
			.And.ContainSingle(m => (m as NameMatcher)!.Name == "otherName");
	}
}