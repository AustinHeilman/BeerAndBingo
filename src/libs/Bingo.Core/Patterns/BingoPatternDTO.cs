namespace Bingo.Core.Patterns;

public class PatternJsonModel
{
	public string PatternName { get; set; } = string.Empty;
	public string? Category { get; set; }
	public bool[][] Pattern { get; set; } = Array.Empty<bool[]>();
}
