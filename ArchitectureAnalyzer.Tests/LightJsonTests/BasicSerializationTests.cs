using LightJson;

namespace ArchitectureAnalyzer.Tests.LightJsonTests;

public sealed class BasicSerializationTests
{
	[Fact]
	public void ParseExampleMessage()
	{
		var message = @"
				{
					""menu"": [
						""home"",
						""projects"",
						""about""
					]
				}
			";

		var json = JsonValue.Parse(message);

		json.IsJsonObject.Should().BeTrue();

		json.AsJsonObject!.Count.Should().Be(1);
		json.AsJsonObject.ContainsKey("menu").Should().BeTrue();

		var items = json.AsJsonObject["menu"].AsJsonArray;

		items.Should()
			.NotBeNull()
			.And.HaveCount(3)
			.And.Contain("home")
			.And.Contain("projects")
			.And.Contain("about");
	}
}