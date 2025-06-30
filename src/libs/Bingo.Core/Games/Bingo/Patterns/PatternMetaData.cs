namespace Bingo.Core.Games.Bingo.Patterns
{
	public class PatternMetadata
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? Difficulty { get; set; }  // e.g., "Easy", "Hard", "Advanced"
		public bool IsSymmetrical { get; set; }
		public bool IsDefault { get; set; }
	}
}

