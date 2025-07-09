using Bingo.Core.Patterns;

namespace Bingo.Core.Messaging;

public class SavePatternMessage
{
	public BingoPattern Pattern { get; }
	public SavePatternMessage(BingoPattern pattern) => Pattern = pattern;
}
