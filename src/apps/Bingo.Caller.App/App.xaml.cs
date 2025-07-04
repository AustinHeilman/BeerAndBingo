using Bingo.Caller.App.Startup;
using CommunityToolkit.Mvvm.DependencyInjection;
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
		if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst())
		{
			await PatternInstaller.InstallPatternsIfFirstLaunchAsync();
		}

#if DEBUG
		try
		{
			//var patternService = Ioc.Resolve<IPatternService>(); // adjust if you're using DI directly
			// DI the pattern repository service?
			var patterns = await patternService.GetPatternNamesAsync();

			Debug.WriteLine(" Saved Patterns on Startup:");
			foreach (var name in patterns)
				Debug.WriteLine($"   - {name}");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($" Error loading saved patterns: {ex.Message}");
		}
#endif

		await Task.Delay(1);
	}
}