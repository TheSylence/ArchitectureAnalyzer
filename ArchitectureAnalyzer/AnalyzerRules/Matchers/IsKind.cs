namespace ArchitectureAnalyzer.AnalyzerRules.Matchers;

[Flags]
public enum IsKind
{
	None = 0,

	Class = 0x0001,
	Interface = 0x0002,
	Struct = 0x0004,
	Enum = 0x0008,

	Abstract = 0x0010,
	Sealed = 0x0020,
	Static = 0x0040,

	Public = 0x0100,
	Private = 0x0200,
	Protected = 0x0400,
	Internal = 0x0800
}