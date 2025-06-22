using Bingo.AppServices.Patterns;
using Bingo.Caller.App.Startup;

namespace Bingo.Caller.App;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        this.AddCustomResources();
        // Optional: add services or setup logic here
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var mainPage = _serviceProvider.GetRequiredService<MainPage>();
        Window window = new(mainPage);
        return window;
    }

    protected override async void OnStart()
    {
        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst())
        {
            // To-do: Install the default patterns if not already installed
        }

        await Task.Delay(1);
    }
}