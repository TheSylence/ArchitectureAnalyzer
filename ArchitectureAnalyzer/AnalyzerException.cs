namespace ArchitectureAnalyzer;

internal sealed class AnalyzerException : Exception
{
	public AnalyzerException(string message)
		: base(message)
	{
	}
}