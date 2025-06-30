using Bingo.ViewModels;
using Microsoft.Maui.Controls;

namespace Bingo.UI.Shared.Views.GameInfoPanel
{
	public partial class GameInfoPanel : ContentView
	{
		public GameInfoPanel()
		{
			InitializeComponent();
			BindingContext = new GameInfoPanelViewModel();
		}
	}
}
