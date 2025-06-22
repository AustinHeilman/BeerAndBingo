using System.Collections.Generic;
using System.Linq;
using Bingo.ModelView.FlashBoard;
using Xunit;

namespace Bingo.ViewModel.Tests.FlashBoard;

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
        vm.CalledNumbers.Add(17);

        vm.ToggleCallCommand.Execute(17);

        Assert.DoesNotContain(17, vm.CalledNumbers);
    }

    [Fact]
    public void CompletedColumns_ReturnsCorrectColumnLetter_WhenAllColumnNumbersAreCalled()
    {
        var vm = new FlashBoardViewModel();

        // Complete the "B" column: 1, 16, 31, 46, 61
        var bColumn = new[] { 1, 16, 31, 46, 61 };
        foreach (var num in bColumn)
            vm.CalledNumbers.Add(num);

        var completed = vm.CompletedColumns.ToList();

        Assert.Contains('B', completed);
        Assert.DoesNotContain('I', completed);
        Assert.Single(completed);
    }

    [Fact]
    public void CompletedColumns_IsEmpty_WhenColumnsAreIncomplete()
    {
        var vm = new FlashBoardViewModel();

        // Only part of "N" column
        vm.CalledNumbers.Add(3);   // B column
        vm.CalledNumbers.Add(33);  // N column

        var completed = vm.CompletedColumns;

        Assert.Empty(completed);
    }
}
