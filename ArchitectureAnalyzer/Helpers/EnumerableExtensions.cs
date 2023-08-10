namespace ArchitectureAnalyzer.Helpers;

internal static class EnumerableExtensions
{
	public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
	{
		foreach (var item in items)
		{
			if (item is not null)
				yield return item;
		}
	}
}