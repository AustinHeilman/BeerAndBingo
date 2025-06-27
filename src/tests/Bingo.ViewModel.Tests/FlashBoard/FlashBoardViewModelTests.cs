using Bingo.ViewModel.FlashBoard;

namespace Bingo.ViewModel.Tests.FlashBoard
{
	public class FlashBoardViewModelTests
	{
		[Fact]
		public void ToggleCallCommand_AddsNumber_WhenNotAlreadyCalled()
		{
			FlashBoardViewModel vm = new();

			vm.ToggleCallCommand.Execute(42);

			Assert.Contains(42, vm.CalledNumbers);
		}

		[Fact]
		public void ToggleCallCommand_RemovesNumber_WhenAlreadyCalled()
		{
			FlashBoardViewModel vm = new();
			vm.ToggleCallCommand.Execute(17); // Call once
			vm.ToggleCallCommand.Execute(17); // Call again to uncall

			Assert.DoesNotContain(17, vm.CalledNumbers);
		}

		[Fact]
		public void CompletedColumns_ReturnsCorrectColumn_WhenAllItsNumbersAreCalled()
		{
			FlashBoardViewModel vm = new();
			FlashBoardGroupViewModel bGroup = vm.Groups.First(g => g.Letter == 'B');

			foreach (FlashBoardCellViewModel cell in bGroup.Cells)
				vm.CallNumber(cell.Number);

			List<char> completed = vm.CompletedColumns.ToList();

			Assert.Single(completed);
			Assert.Contains('B', completed);
		}

		[Fact]
		public void CompletedColumns_Empty_WhenNoColumnIsFullyCalled()
		{
			FlashBoardViewModel vm = new();
			vm.CallNumber(3);   // Likely B-column
			vm.CallNumber(33);  // Likely N-column

			IEnumerable<char> completed = vm.CompletedColumns;

			Assert.Empty(completed);
		}

		[Fact]
		public void BColumnNumbers_ShouldBelongToGroupB()
		{
			FlashBoardViewModel vm = new();
			FlashBoardGroupViewModel bGroup = vm.Groups.First(g => g.Letter == 'B');

			foreach (FlashBoardCellViewModel cell in bGroup.Cells)
				Assert.Equal('B', cell.Parent.Letter);
		}

		[Fact]
		public void AllCalled_BGroup_Yields_CompletedColumnB()
		{
			FlashBoardViewModel vm = new();
			FlashBoardGroupViewModel bGroup = vm.Groups.First(g => g.Letter == 'B');

			foreach (FlashBoardCellViewModel cell in bGroup.Cells)
				vm.CallNumber(cell.Number);

			List<char> completed = vm.CompletedColumns.ToList();

			Assert.Contains('B', completed);
		}

		[Fact]
		public void AllCalled_ThenUncall_ColumnIsNoLongerCompleted()
		{
			FlashBoardViewModel vm = new();
			FlashBoardGroupViewModel bGroup = vm.Groups.First(g => g.Letter == 'B');

			foreach (FlashBoardCellViewModel cell in bGroup.Cells)
				vm.CallNumber(cell.Number);

			// Uncall one number
			int toUncall = bGroup.Cells.First().Number;
			vm.UncallNumber(toUncall);

			IEnumerable<char> completed = vm.CompletedColumns;

			Assert.DoesNotContain('B', completed);
		}

		[Theory]
		[InlineData('B')]
		[InlineData('I')]
		[InlineData('N')]
		[InlineData('G')]
		[InlineData('O')]
		public void EachGroup_IsCompleted_WhenAllNumbersAreCalled(char letter)
		{
			FlashBoardViewModel vm = new();
			FlashBoardGroupViewModel group = vm.Groups.First(g => g.Letter == letter);

			foreach (FlashBoardCellViewModel cell in group.Cells)
				vm.CallNumber(cell.Number);

			IEnumerable<char> completed = vm.CompletedColumns;

			Assert.Contains(letter, completed);
		}
	}
}
