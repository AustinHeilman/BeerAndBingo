using Bingo.Core.Patterns;

namespace Bingo.Services.Patterns.Events;

/// <summary>
/// Fired when the active pattern is updated or cleared.
/// `Pattern` will never be null—defaults to `BingoPattern.EmptyPattern`.
/// </summary>
public sealed class PatternChangedEvent
{
	public BingoPattern Pattern { get; }

	public PatternChangedEvent(BingoPattern? pattern)
	{
		Pattern = pattern ?? BingoPattern.EmptyPattern;
	}
}
