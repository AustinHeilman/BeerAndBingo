using System.Text;

namespace Bingo.Core.Domain.FlashBoard;

public class FlashBoardObj
{    
    public List<FlashBoardGroup> Children { get; private set; } = new();

    public FlashBoardObj()
    {
        BuildLetterGroups();
    }

    private void BuildLetterGroups()
    {
        Children = new();
        foreach (var letter in FlashBoardConfig.GameLetters)
        {
            var numbers = FlashBoardConfig.GetNumbersInBoardLetter(letter);
            int start = numbers.First();
            int end = numbers.Last();

            var group = new FlashBoardGroup(letter, start, end, this);
            Children.Add(group);
        }

        ToTextBoard();

    }

    public string ToTextBoard()
    {
        StringBuilder sb = new();
        foreach ( var group in Children )
        {
            sb.Append($"{group.Letter}: ");
            foreach (var cell in group.Cells)
            {
                sb.Append(cell.IsCalled ? $"{cell.Number}* " : $"{cell.Number} ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public FlashBoardNumber? GetCell(char letter, int number) =>
        Children.FirstOrDefault(g => g.Letter == letter)?.Cells.FirstOrDefault(c => c.Number == number);

    public IEnumerable<FlashBoardNumber> AllCells => Children.SelectMany(g => g.Cells);

    public IEnumerable<int> CalledNumbers => AllCells.Where(c => c.IsCalled).Select(c => c.Number);

    public void CallNumber(int number)
    {
        var cell = AllCells.FirstOrDefault(c => c.Number == number);
        if (cell != null)
            cell.IsCalled = true;
    }

    public void UncallNumber(int number)
    {
        var cell = AllCells.FirstOrDefault(c => c.Number == number);
        if (cell != null)
            cell.IsCalled = false;
    }

    public IEnumerable<char> CompletedColumns()
    {
        foreach (var group in Children)
        {
            if (group.Cells.All(c => c.IsCalled))
                yield return group.Letter;
        }
    }

    public bool IsValidNumber(int number) =>
        number >= 1 && number <= FlashBoardConfig.TotalNumbers;

    public bool Contains(char letter, int number) =>
        GetCell(letter, number) is not null;
}
