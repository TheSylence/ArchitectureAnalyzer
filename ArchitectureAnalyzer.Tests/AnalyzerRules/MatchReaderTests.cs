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