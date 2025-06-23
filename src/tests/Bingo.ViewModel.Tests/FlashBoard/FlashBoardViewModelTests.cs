using System.Linq;
using Xunit;
using Bingo.ViewModel.FlashBoard;

namespace Bingo.ViewModel.Tests.FlashBoard
{
    public class FlashBoardViewModelTests
    {
        [Fact]
        public void ToggleCallCommand_AddsNumber_WhenNotAlreadyCalled()
        {
            var vm = new FlashBoardViewModel();

            vm.ToggleCallCommand.Execute(42);

            Assert.Contains(42, vm.CalledNumbers);
        }

        [Fact]
        public void ToggleCallCommand_RemovesNumber_WhenAlreadyCalled()
        {
            var vm = new FlashBoardViewModel();
            vm.ToggleCallCommand.Execute(17); // Call once
            vm.ToggleCallCommand.Execute(17); // Call again to uncall

            Assert.DoesNotContain(17, vm.CalledNumbers);
        }

        [Fact]
        public void CompletedColumns_ReturnsCorrectColumn_WhenAllItsNumbersAreCalled()
        {
            var vm = new FlashBoardViewModel();
            var bGroup = vm.Groups.First(g => g.Letter == 'B');

            foreach (var cell in bGroup.Cells)
                vm.CallNumber(cell.Number);

            var completed = vm.CompletedColumns.ToList();

            Assert.Single(completed);
            Assert.Contains('B', completed);
        }

        [Fact]
        public void CompletedColumns_Empty_WhenNoColumnIsFullyCalled()
        {
            var vm = new FlashBoardViewModel();
            vm.CallNumber(3);   // Likely B-column
            vm.CallNumber(33);  // Likely N-column

            var completed = vm.CompletedColumns;

            Assert.Empty(completed);
        }

        [Fact]
        public void BColumnNumbers_ShouldBelongToGroupB()
        {
            var vm = new FlashBoardViewModel();
            var bGroup = vm.Groups.First(g => g.Letter == 'B');

            foreach (var cell in bGroup.Cells)
                Assert.Equal('B', cell.Parent.Letter);
        }

        [Fact]
        public void AllCalled_BGroup_Yields_CompletedColumnB()
        {
            var vm = new FlashBoardViewModel();
            var bGroup = vm.Groups.First(g => g.Letter == 'B');

            foreach (var cell in bGroup.Cells)
                vm.CallNumber(cell.Number);

            var completed = vm.CompletedColumns.ToList();

            Assert.Contains('B', completed);
        }

        [Fact]
        public void AllCalled_ThenUncall_ColumnIsNoLongerCompleted()
        {
            var vm = new FlashBoardViewModel();
            var bGroup = vm.Groups.First(g => g.Letter == 'B');

            foreach (var cell in bGroup.Cells)
                vm.CallNumber(cell.Number);

            // Uncall one number
            var toUncall = bGroup.Cells.First().Number;
            vm.UncallNumber(toUncall);

            var completed = vm.CompletedColumns;

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
            var vm = new FlashBoardViewModel();
            var group = vm.Groups.First(g => g.Letter == letter);

            foreach (var cell in group.Cells)
                vm.CallNumber(cell.Number);

            var completed = vm.CompletedColumns;

            Assert.Contains(letter, completed);
        }
    }
}
