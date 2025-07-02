using Bingo.ViewModel.GameInfo;

namespace Bingo.UI.Shared.Views.GameInfoPanel;

public partial class GameInfoPanel : ContentView
{
	public GameInfoPanel()
	{
		InitializeComponent();

		// This will be ignored if BindingContext is overridden in MainPage.xaml.cs
		BindingContext = new GameInfoPanelViewModel(new Bingo.Core.Domain.Bingo.BingoSession());
	}
}
