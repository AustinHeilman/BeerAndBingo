namespace Bingo.Core.Patterns.Utility;

public static class RandomPatternGenerator
{
	private static readonly Random Rng = new();

	private static readonly string[] Prefixes = { "Wiggly", "Turbo", "Sneaky", "Quantum", "Lucky", "Zebra", "Shadow", "Emperor", "Damned" };
	private static readonly string[] Nouns = { "Waffle", "Rocket", "Goose", "Banana", "Fork", "Spoon", "Car", "Missionary", "FleshyPickle", "Meteor", "Slug", "Number" };
	private static readonly string[] Suffixes = { "", "Jr", "XL", "Prime", "of Doom", "2000", "Void", "Sr", "The Great", "67" };

	public static string RandomPatternName()
	{
		string prefix = Prefixes[Rng.Next(Prefixes.Length)];
		string noun = Nouns[Rng.Next(Nouns.Length)];
		string suffix = Suffixes[Rng.Next(Suffixes.Length)];

		return string.IsNullOrWhiteSpace(suffix)
			? $"{prefix} {noun}"
			: $"{prefix} {noun} {suffix}";
	}

	public static HashSet<PatternCell> RandomPatternSquares(int rows = 5, int cols = 5, double fillRatio = 0.3)
	{
		HashSet<PatternCell> cells = new();
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (Rng.NextDouble() < fillRatio)
					cells.Add(new PatternCell { Row = row, Col = col, IsActive = true });
			}
		}
		return cells;
	}

	public static BingoPattern GenerateRandomPattern(int rows = 5, int cols = 5, double fillRatio = 0.3)
	{
		string name = RandomPatternName();
		HashSet<PatternCell> cells = RandomPatternSquares(rows, cols, fillRatio);

		return new BingoPattern
		{
			Name = name,
			Cells = cells
		};
	}

}
