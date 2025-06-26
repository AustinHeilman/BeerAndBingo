namespace Bingo.Core.Patterns;

public class PatternCell
{
	public int Row { get; set; }
	public int Col { get; set; }
	public bool IsActive { get; set; }

	public void Deconstruct(out int row, out int col)
	{
		row = Row;
		col = Col;
	}
}
