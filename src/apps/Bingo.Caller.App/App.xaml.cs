using Bingo.Caller.App.Startup;

namespace Bingo.Caller.App;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_serviceProvider = serviceProvider;

		if (Application.Current != null && Application.Current is App app)
		{
			Application.Current.UserAppTheme = AppTheme.Dark;

			app.AddCustomResources(); // Ensure resources are applied on startup

			Application.Current.RequestedThemeChanged += (_, args) =>
			{
				(Application.Current as App)?.AddCustomResources(); // Reapply styles
			};
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		MainPage mainPage = _serviceProvider.GetRequiredService<MainPage>();
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