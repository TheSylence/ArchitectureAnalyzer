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
	public void Ignores_NonObjectRules()
	{
		// Arrange
		const string json = """{ "rules": [1, 2, 3] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().BeEmpty();
	}

	[Fact]
	public void IsEmpty_WhenNoRulesArePresent()
	{
		// Arrange
		const string json = """{ "rules": [] }""";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().BeEmpty();
	}

	[Fact]
	public void IsEmpty_WhenRuleElementIsMissing()
	{
		// Arrange
		const string json = "{ }";

		// Act
		var actual = _sut.Read(json).ToList();

		// Assert
		actual.Should().BeEmpty();
	}

	[Fact]
	public void MustImplement_Throws_WhenInterfaceIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustImplement": { "forTypes": {} } }]}""";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<AnalyzerException>().WithMessage("*interface*");
	}

	[Fact]
	public void MustInherit_Throws_WhenBaseTypeIsMissing()
	{
		// Arrange
		const string json = """{ "rules": [{ "mustInherit": { "forTypes": {} } }]}""";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<AnalyzerException>().WithMessage("*baseType*");
	}

	[Fact]
	public void MustReference_Throws_WhenReferenceIsMissing()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "mustReference": { "forTypes": {} } }] }""";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<AnalyzerException>().WithMessage("*reference*");
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
	public void RelatedTypeExists_Throws_WhenRelatedTypeIsMissing()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "relatedTypeExists": { "forTypes": {} } }] }""";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<AnalyzerException>().WithMessage("*relatedType*");
	}

	[Fact]
	public void Throws_WhenJsonIsInvalid()
	{
		// Arrange
		const string json = "invalid json";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<Exception>();
	}

	[Fact]
	public void Throws_WhenRuleTypeIsUnknown()
	{
		// Arrange
		const string json =
			"""{ "rules": [{ "unknownRuleType": {}}] }""";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<Exception>();
	}

	[Theory]
	[InlineData("mustImplement")]
	[InlineData("mustInherit")]
	[InlineData("relatedTypeExists")]
	[InlineData("mustReference")]
	public void Throws_WhenTypeMatcherIsMissing(string rule)
	{
		// Arrange
		var json = "{ \"rules\": [{ \"" + rule + "\": {} }]}";

		// Act
		var action = () => _sut.Read(json).ToList();

		// Assert
		action.Should().Throw<AnalyzerException>().WithMessage("*forTypes*");
	}
}