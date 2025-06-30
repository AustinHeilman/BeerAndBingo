using Bingo.AppServices.Patterns;
using Bingo.ViewModel.FlashBoard;
using Bingo.ViewModel.GameInfo;
using Bingo.ViewModel.Patterns;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Bingo.ViewModel.MainPage.Caller;

public class CallerMainPageViewModel : ObservableObject
{
	public FlashBoardViewModel FlashBoardVM { get; } = new();
	public PatternDisplayViewModel PatternVM { get; }
	public GameInfoPanelViewModel GameInfoVM { get; } = new();

	public CallerMainPageViewModel()
	{
		DefaultPatternRepository repo = new();

		PatternVM = new PatternDisplayViewModel(repo);

		// Ensure a visible grid with no active cells on launch
		_ = PatternVM.LoadPatternAsync("None");

		GameInfoVM.CurrentCallDisplay = "G 57";
		GameInfoVM.PreviousCallDisplay = "I 29";
		GameInfoVM.GameRoundsText = "Rounds: 2";
		GameInfoVM.FakeCallDisplay = "Z 26 FAKE BINGO CALLED!";
		GameInfoVM.ShowFakeCall = true;
	}
}
