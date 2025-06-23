namespace Bingo.Core.Domain.FlashBoard;

public class FlashBoardGroup
{
    public char Letter { get; }
    public FlashBoard? Parent { get; internal set; }

    public List<FlashBoardNumber> Cells { get; } = new();

    public FlashBoardGroup(char letter, int start, int end, FlashBoard parent)
    {
        Letter = letter;
        Parent = parent;

        for (int number = start; number <= end; number++)
        {
            var cell = new FlashBoardNumber(number, this);            
            Cells.Add(cell);
        }
    }

    public int Index => Parent?.Children.IndexOf(this) ?? -1;

    public int GetCellIndex(FlashBoardNumber cell) => Cells.IndexOf(cell);

    public int Count => Cells.Count;
}
