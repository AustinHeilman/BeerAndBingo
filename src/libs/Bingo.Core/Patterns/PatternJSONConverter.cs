namespace Bingo.Core.Patterns;

public static class PatternJsonConverter
{
	public static BingoPattern ToDomain(this PatternJsonModel dto)
	{
		var pattern = new BingoPattern
		{
			Name = dto.PatternName
		};

		for (int row = 0; row < dto.Pattern.Length; row++)
		{
			for (int col = 0; col < dto.Pattern[row].Length; col++)
			{
				if (dto.Pattern[row][col])
				{
					pattern.Cells.Add(new PatternCell
					{
						Row = row,
						Col = col,
						IsActive = true
					});
				}
			}
		}

		return pattern;
	}

	public static PatternJsonModel ToJsonModel(this BingoPattern pattern)
	{
		const int gridSize = 5; // Assuming 5x5

		bool[][] grid = Enumerable.Range(0, gridSize)
			.Select(_ => new bool[gridSize])
			.ToArray();

		foreach (var cell in pattern.GetActiveCells())
		{
			if (cell.Row < gridSize && cell.Col < gridSize)
				grid[cell.Row][cell.Col] = true;
		}

		return new PatternJsonModel
		{
			PatternName = pattern.Name,
			Pattern = grid
		};
	}
}
