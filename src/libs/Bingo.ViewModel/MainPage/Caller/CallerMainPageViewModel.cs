using Bingo.AppServices.Patterns;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.Patterns;
using Microsoft.Extensions.Logging.Abstractions;
namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel
{
    public FlashBoardViewModel FlashBoardVM { get; } = new();
    public PatternDisplayViewModel PatternVM { get; }

    public CallerMainPageViewModel()
    {
        PatternVM = new PatternDisplayViewModel(new DefaultPatternRepository(NullLogger<DefaultPatternRepository>.Instance));

        // Just to test initial state
        FlashBoardVM.CallNumber(7);
        FlashBoardVM.CallNumber(23);
        FlashBoardVM.CallNumber(68);
    }
}
