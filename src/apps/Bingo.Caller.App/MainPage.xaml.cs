using Microsoft.Maui.Controls;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new CallerMainPageViewModel();
    }
}
