using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.UI.Shared.Views.GameInfoPanel;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    private bool _isToolsPanelVisible = false;

    public bool IsToolsPanelVisible
    {
        get => _isToolsPanelVisible;
        set
        {
            if (_isToolsPanelVisible != value)
            {
                _isToolsPanelVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToolsPanelToggleText));
            }
        }
    }

    public string ToolsPanelToggleText => IsToolsPanelVisible ? "Hide Tools" : "Show Tools";

	public MainPage(FlashBoardView flashBoardView, CallerMainPageViewModel viewModel)
	{
		InitializeComponent();

		// Inject FlashBoardView
		flashBoardView.IsInteractive = true;
		flashBoardView.ViewModel = viewModel.FlashBoardVM;
		MainGrid.Children.Add(flashBoardView);
		Grid.SetRow(flashBoardView, 0);

		BindingContext = viewModel;
	}


	private void OnToggleToolsPanelClicked(object sender, EventArgs e)
    {
        IsToolsPanelVisible = !IsToolsPanelVisible;
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
