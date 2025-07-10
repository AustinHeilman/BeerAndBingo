using Bingo.Caller.App.Startup;
using Bingo.Services.Patterns;
using System.Diagnostics;

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
		var repository = _serviceProvider.GetRequiredService<FilePatternRepository>();
		await repository.InitializeAsync(); // Make sure the repository is initialized before use
		await Task.Delay(150); // Give some time for the repository to be ready
		await DefaultPatternInstaller.InstallPatternsIfFirstLaunchAsync(repository);
		await Task.Delay(10);
	}
}