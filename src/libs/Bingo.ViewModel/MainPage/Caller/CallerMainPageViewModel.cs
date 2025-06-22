using Bingo.AppServices.Patterns;
using Bingo.ModelView.FlashBoard;
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
        FlashBoardVM.CalledNumbers.Add(7);
        FlashBoardVM.CalledNumbers.Add(23);
        FlashBoardVM.CalledNumbers.Add(68);
    }
}
