using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bingo.UI.Shared.Views.FlashBoard;
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

        // Set up the FlashBoardView        
        flashBoardView.IsInteractive = true;
        flashBoardView.VerticalOptions = LayoutOptions.Fill;
        flashBoardView.HorizontalOptions = LayoutOptions.Fill;
        flashBoardView.ViewModel = viewModel.FlashBoardVM;

        // Add FlashBoardView to the MainGrid at row 0
        MainGrid.Children.Add(flashBoardView);
        Grid.SetRow(flashBoardView, 0);

        BindingContext = viewModel;
    }

    private void OnToggleToolsPanelClicked(object sender, EventArgs e)
    {
        IsToolsPanelVisible = !IsToolsPanelVisible;
    }

    public new event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
