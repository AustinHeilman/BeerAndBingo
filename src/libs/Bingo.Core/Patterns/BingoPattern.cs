namespace Bingo.Core.Patterns;

public class BingoPattern
{
	public string Name { get; set; } = string.Empty;

	// Core pattern cells
	public HashSet<PatternCell> Cells { get; set; } = new();

	// Quick-access: (row, col) -> cell
	public Dictionary<(int, int), PatternCell> CellMap =>
		Cells.ToDictionary(c => (c.Row, c.Col));

	// Get only active cells (readonly)
	public IEnumerable<PatternCell> GetActiveCells() =>
		Cells.Where(c => c.IsActive);

	// Toggle IsActive at a given cell (adds it if missing)
	public void ToggleCell(int row, int col)
	{
		PatternCell? match = Cells.FirstOrDefault(c => c.Row == row && c.Col == col);
		if (match is not null)
		{
			match.IsActive = !match.IsActive;
		}
		else
		{
			Cells.Add(new PatternCell
			{
				Row = row,
				Col = col,
				IsActive = true
			});
		}
	}

	// Deep clone: copies all metadata and cell state
	public BingoPattern Clone()
	{
		return new BingoPattern
		{
			Name = this.Name,
			Cells = this.Cells
				.Select(c => new PatternCell
				{
					Row = c.Row,
					Col = c.Col,
					IsActive = c.IsActive
				})
				.ToHashSet()
		};
	}

	// Used for caller preview optimization
	public HashSet<int> GetUsedColumns() =>
		GetActiveCells().Select(c => c.Col).ToHashSet();

	public static BingoPattern EmptyPattern => new()
	{
		Name = "None",
		Cells = new HashSet<PatternCell>
		(
			Enumerable.Range(0, PatternGridSettings.PatternRowCount)
				.SelectMany(row => Enumerable.Range(0, PatternGridSettings.PatternColCount)
					.Select(col => new PatternCell
					{
						Row = row,
						Col = col,
						IsActive = false// Random.Shared.Next(2) == 0 // 50/50 chance
					})
			)
		)
	};
}
