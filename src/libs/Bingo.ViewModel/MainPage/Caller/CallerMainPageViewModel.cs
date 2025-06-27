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
		DefaultPatternRepository repo = new();

		PatternVM = new PatternDisplayViewModel(repo);

		// Ensure a visible grid with no active cells on launch
		_ = PatternVM.LoadPatternAsync("None");
	}
}
