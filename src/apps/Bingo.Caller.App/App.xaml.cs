using Bingo.Caller.App.Startup;
using Bingo.Core.Patterns;
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
		if (OperatingSystem.IsWindows() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst())
		{
			try
			{
				var repository = _serviceProvider.GetRequiredService<FilePatternRepository>();
				await DefaultPatternInstaller.InstallPatternsIfFirstLaunchAsync(repository);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[App.OnStart] Pattern installation failed: {ex.Message}");
			}
		}

#if DEBUG
		try
		{
			var patternRepo = _serviceProvider.GetRequiredService<PatternRepositoryBase>();
			var patterns = await patternRepo.LoadAllFromFilesAsync();

			Debug.WriteLine("[App.OnStart] Saved saved patterns on Startup list:");
			foreach (var pattern in patterns)
				Debug.WriteLine($"   - {pattern.Name}");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error loading saved patterns: {ex.Message}");
		}
#endif


		await Task.Delay(50); // Prevents thread hiccups; safe as a stub
	}
}