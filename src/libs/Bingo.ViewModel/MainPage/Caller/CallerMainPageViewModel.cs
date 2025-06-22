using Bingo.AppServices.Patterns;
using Bingo.ViewModel.Patterns;
using Microsoft.Extensions.Logging.Abstractions;
namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel
{
    public PatternDisplayViewModel PatternVM { get; }

    public CallerMainPageViewModel()
    {
        var repo = new DefaultPatternRepository(NullLogger<DefaultPatternRepository>.Instance);
        PatternVM = new PatternDisplayViewModel(repo);

        _ = PatternVM.LoadPatternAsync("FourCorners");
    }
}
