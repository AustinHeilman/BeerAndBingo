using Microsoft.Maui.Controls;
using Bingo.ViewModel.MainPage.Caller;
using Bingo.UI.Shared.Views.FlashBoard;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
    private readonly FlashBoardView _flashBoardView;

    public MainPage(FlashBoardView flashBoardView)
    {
        InitializeComponent();

        _flashBoardView = flashBoardView;

        BindingContext = new CallerMainPageViewModel();
    }
}
