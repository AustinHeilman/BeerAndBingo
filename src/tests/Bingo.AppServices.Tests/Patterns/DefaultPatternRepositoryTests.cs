using Bingo.AppServices.Patterns;
using Bingo.Core.Patterns;
using Microsoft.Extensions.Logging;

namespace Bingo.AppServices.Tests.Patterns;

public class DefaultPatternRepositoryTests
{
	[Fact]
	public async Task LoadsOnlyValidPatterns_WhenMalformedIncluded()
	{
		string json = """
        [
            { "PatternName": "Valid", "Pattern": [
                [ true, false, false, false, true ],
                [ false, false, false, false, false ],
                [ false, false, false, false, false ],
                [ false, false, false, false, false ],
                [ true, false, false, false, true ]
            ]},
            { "PatternName": "", "Pattern": [[false]] },
            { "PatternName": "Empty", "Pattern": [
                [ false, false, false, false, false ],
                [ false, false, false, false, false ],
                [ false, false, false, false, false ],
                [ false, false, false, false, false ],
                [ false, false, false, false, false ]
            ]}
        ]
        """;

		TestLogger<DefaultPatternRepository> logger = new();
		using MemoryStream stream = new(System.Text.Encoding.UTF8.GetBytes(json));
		using StreamReader reader = new(stream);
		string jsonText = reader.ReadToEnd();

		// Inject the JSON stream manually
		TestableDefaultPatternRepository repository = new(jsonText, logger);

		List<BingoPattern> all = (await repository.GetAllAsync()).ToList();

		Assert.Single(all); // Only "Valid" should remain
		Assert.Equal("Valid", all[0].Name);
		Assert.Contains(logger.Messages, m => m.Contains("Discarded invalid pattern"));
	}

	// Explicitly implement the interface method to match nullability constraints
	private class TestLogger<T> : ILogger<T>
	{
		public List<string> Messages { get; } = new();

		IDisposable ILogger.BeginScope<TState>(TState state) => null!;
		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(
			LogLevel logLevel, EventId eventId,
			TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			Messages.Add(formatter(state, exception));
		}
	}

	// Custom version to inject fake JSON
	private class TestableDefaultPatternRepository : DefaultPatternRepository
	{
		public TestableDefaultPatternRepository(string json, ILogger<DefaultPatternRepository> logger)
			: base(json, logger) { }
	}
}
