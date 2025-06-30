using CommunityToolkit.Mvvm.ComponentModel;

namespace Bingo.ViewModel.GameInfo;

public partial class GameInfoPanelViewModel : ObservableObject
{
	[ObservableProperty]
	private string currentCallDisplay = "G 57";

	[ObservableProperty]
	private string previousCallDisplay = "I 29";

	[ObservableProperty]
	private string gameRoundsText = "Rounds: 2";

	[ObservableProperty]
	private bool showFakeCall = true;

	[ObservableProperty]
	private string fakeCallDisplay = "Z 26 FAKE BINGO CALLED!";
}
