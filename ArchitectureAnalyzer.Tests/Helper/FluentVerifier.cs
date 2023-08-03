using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace ArchitectureAnalyzer.Tests.Helper;

internal sealed class FluentVerifier : IVerifier
{
	public void Empty<T>(string collectionName, IEnumerable<T> collection)
	{
		collection.Should().BeEmpty("because {0} should be empty", collectionName);
	}

	public void Equal<T>(T expected, T actual, string? message = null)
	{
		expected.Should().Be(actual, message);
	}

	[DoesNotReturn]
	public void Fail(string? message = null)
	{
		Assert.Fail(message);
	}

	public void False(bool assert, string? message = null)
	{
		assert.Should().BeFalse(message);
	}

	public void LanguageIsSupported(string language)
	{
		if (language != LanguageNames.CSharp)
			throw new NotSupportedException($"Language {language} is not supported");
	}

	public void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
	{
		collection.Should().NotBeEmpty("because {0} should not be empty", collectionName);
	}

	public IVerifier PushContext(string context)
	{
		return this;
	}

	public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual,
		IEqualityComparer<T>? equalityComparer = null,
		string? message = null)
	{
		if (equalityComparer is null)
			expected.Should().Equal(actual, message);
		else
			expected.Should().Equal(actual, equalityComparer.Equals, message);
	}

	public void True(bool assert, string? message = null)
	{
		assert.Should().BeTrue(message);
	}
}