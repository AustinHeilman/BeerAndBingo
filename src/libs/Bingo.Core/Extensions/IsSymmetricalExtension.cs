using Bingo.Core.Patterns;

namespace Bingo.Core.Extensions;

public static class IsSymmetricalExtension
{
	public enum SymmetryKind { Horizontal, Vertical, Diagonal }

	public static bool IsSymmetrical(this BingoPattern pattern, SymmetryKind kind)
	{
		int maxRow = PatternGridSettings.PatternRowCount - 1;
		int maxCol = PatternGridSettings.PatternColCount - 1;

		return kind switch
		{
			SymmetryKind.Horizontal =>
				pattern.CellMap.All(pair =>
					pattern.CellMap.ContainsKey((pair.Key.Item1, maxCol - pair.Key.Item2)) &&
					pattern.CellMap[(pair.Key.Item1, maxCol - pair.Key.Item2)].IsActive == pair.Value.IsActive),

			SymmetryKind.Vertical =>
				pattern.CellMap.All(pair =>
					pattern.CellMap.ContainsKey((maxRow - pair.Key.Item1, pair.Key.Item2)) &&
					pattern.CellMap[(maxRow - pair.Key.Item1, pair.Key.Item2)].IsActive == pair.Value.IsActive),

			SymmetryKind.Diagonal =>
				pattern.CellMap.All(pair =>
					pattern.CellMap.ContainsKey((pair.Key.Item2, pair.Key.Item1)) &&
					pattern.CellMap[(pair.Key.Item2, pair.Key.Item1)].IsActive == pair.Value.IsActive),

			_ => false
		};
	}
}
