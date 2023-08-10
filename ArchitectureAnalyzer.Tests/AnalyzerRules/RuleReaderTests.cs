using ArchitectureAnalyzer.AnalyzerRules;
using ArchitectureAnalyzer.AnalyzerRules.Rules;
using ArchitectureAnalyzer.Tests.AnalyzerRules.Matchers;
using LightJson;

namespace ArchitectureAnalyzer.Tests.AnalyzerRules;

public sealed class RuleReaderTests
{
	public RuleReaderTests()
	{
		var matchReader = Substitute.For<IMatchReader>();
		var mockMatcher = MockMatcher.Create(true);
		matchReader.ReadMatcher(Arg.Any<JsonObject>()).Returns(mockMatcher);

		_sut = new RuleReader(matchReader);
	}

	private readonly RuleReader _sut;

	[Fact]
	public void MustImplement_Throws_WhenInterfaceIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustImplement": { "forTypes": {} } }]}""";

		// Act
		var actual = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		actual.Should().BeOfType<AnalyzerException>().Which.Message.Should().Contain("interface");
	}

	[Fact]
	public void MustImplement_Throws_WhenTypeMatcherIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustImplement": { "interface": {} } }]}""";

		// Act
		var actual = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		actual.Should().BeOfType<AnalyzerException>().Which.Message.Should().Contain("forTypes");
	}

	[Fact]
	public void MustInherit_Throws_WhenBaseTypeIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustInherit": { "forTypes": {} } }]}""";

		// Act
		var actual = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		actual.Should().BeOfType<AnalyzerException>().Which.Message.Should().Contain("baseType");
	}

	[Fact]
	public void MustInherit_Throws_WhenTypeMatcherIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustInherit": { "baseType": {} } }]}""";

		// Act
		var actual = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		actual.Should().BeOfType<AnalyzerException>().Which.Message.Should().Contain("forTypes");
	}

	[Fact]
	public void Reads_MustImplementRule()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "mustImplement": { "forTypes": {}, "interface": {} } }] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().ContainSingle(r => r is MustImplementRule);
	}

	[Fact]
	public void Reads_MustInheritRule()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "mustInherit": { "forTypes":{}, "baseType":{}  } }] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().ContainSingle(r => r is MustInheritRule);
	}

	[Fact]
	public void Reads_MustReferenceRule()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "mustReference": { "forTypes": {}, "reference": {} } }] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().ContainSingle(r => r is MustReferenceRule);
	}

	[Fact]
	public void Reads_RelatedTypeExistsRule()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "relatedTypeExists": { "forTypes": {}, "relatedType": {} } }] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().ContainSingle(r => r is RelatedTypeExistsRule);
	}

	[Fact]
	public void Throws_WhenJsonIsInvalid()
	{
		// Arrange
		const string json = "invalid json";

		// Act
		var ex = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		ex.Should().NotBeNull();
	}

	[Fact]
	public void Throws_WhenRuleTypeIsUnknown()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "unknownRuleType": {}}] }""";

		// Act
		var ex = Record.Exception(() => _sut.Read(json).ToList());

		// Assert
		ex.Should().NotBeNull();
	}
}