namespace Bingo.Core.Domain.FlashBoard;

public class FlashBoardNumber
{
    public int Number { get; }
    public bool IsCalled { get; set; }

    public FlashBoardGroup Parent { get; internal set; }

    public FlashBoard? Board => Parent?.Parent;

    public int ColumnIndex => Board?.Children.IndexOf(Parent) ?? -1;

    public FlashBoardNumber(int number, FlashBoardGroup parent)
    {
        Number = number;
        Parent = parent;
        IsCalled = false;
    }

    public override bool Equals(object? obj) =>
        obj is FlashBoardNumber other && Number == other.Number;

    public override int GetHashCode() => Number.GetHashCode();
}
