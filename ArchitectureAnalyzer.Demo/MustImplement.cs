namespace ArchitectureAnalyzer.Demo;

// --------------------------------------------------
// Demo for the rule: AA0001 MustImplement
// --------------------------------------------------

// This will not trigger any rule violations
public class TestDisposable : IDisposable
{
	public void Dispose() { }
}

// This will trigger a rule violation because the class does not implement IDisposable
public class NonDisposable
{}