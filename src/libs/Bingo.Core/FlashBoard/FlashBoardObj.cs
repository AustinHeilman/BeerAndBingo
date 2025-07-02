using Bingo.Core.FlashBoard.Events;

namespace Bingo.Core.FlashBoard;

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
		foreach (char letter in FlashBoardConfig.GameLetters)
		{
			IEnumerable<int> numbers = FlashBoardConfig.GetNumbersInBoardLetter(letter);
			FlashBoardGroup group = new(letter, numbers.First(), numbers.Last(), this);
			Children.Add(group);
		}
	}

	public IEnumerable<FlashBoardNumber> AllCells => Children.SelectMany(g => g.Cells);

	public IEnumerable<int> CalledNumbers => AllCells.Where(c => c.IsCalled).Select(c => c.Number);

	public FlashBoardNumber? GetCell(char letter, int number) =>
		Children.FirstOrDefault(g => g.Letter == letter)?.Cells.FirstOrDefault(c => c.Number == number);

	public void UpdateCalled(IEnumerable<int> called)
	{
		HashSet<int> calledSet = new(called);
		foreach (FlashBoardNumber cell in AllCells)
			cell.SetCalled(calledSet.Contains(cell.Number), FlashBoardEventSource.Sync);
	}
}
