namespace ArchitectureAnalyzer.Demo;

// --------------------------------------------------
// Demo for the rule: AA0002 MustInherit
// --------------------------------------------------


// This will not trigger any rule violations
public class ValidAttribute : Attribute
{
}

// This will trigger a rule violation because the attribute is not derived from System.Attribute
public class InvalidAttribute
{
}