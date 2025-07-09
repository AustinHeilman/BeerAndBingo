using Bingo.AppServices.Configuration;
using Bingo.Core.Patterns;
using Bingo.Services.Patterns;
using Bingo.UI.Shared.Services;
using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.UI.Shared.Views.Patterns;
using Bingo.ViewModel.MainPage;
using Bingo.ViewModel.Patterns;
using System.Diagnostics;

namespace Bingo.Caller.App.Startup;

public static class ConfigureAppServices
{
	public static MauiAppBuilder AddBeerAndBingoServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<IDeviceInfoProvider, MauiDeviceInfoProvider>();
		builder.Services.AddSingleton<StyleBindingService>();
		builder.Services.AddTransient<FlashBoardView>();
		builder.Services.AddTransient<CallerMainPageViewModel>();
		builder.Services.AddTransient<MainPage>();
		string basePath = FileSystem.AppDataDirectory;

		builder.Services.AddSingleton<FilePatternRepository>(provider =>
		{
			Debug.WriteLine($"[ConfigureAppServices.AddBeerAndBingoServices] Using base path for patterns: {basePath}");
			//string patternPath = Path.Combine(basePath, "Patterns");
			return new FilePatternRepository(basePath);
		});
		builder.Services.AddSingleton<PatternRepositoryBase>(provider => provider.GetRequiredService<FilePatternRepository>());

		builder.Services.AddTransient<CreatePatternView>();
		builder.Services.AddTransient<PatternMainPageViewModel>();
		builder.Services.AddTransient<PatternMainPageView>();
		builder.Services.AddTransient<LoadPatternViewModel>(provider =>
		{
			Debug.WriteLine("[DI] Constructing LoadPatternViewModel");
			return new LoadPatternViewModel(provider.GetRequiredService<PatternRepositoryBase>());
		});
		builder.Services.AddTransient<LoadPatternView>(provider =>
		{
			Debug.WriteLine("[DI] Constructing LoadPatternView");
			var repo = provider.GetRequiredService<PatternRepositoryBase>();
			var vm = provider.GetRequiredService<LoadPatternViewModel>();
			return new LoadPatternView(vm, repo); // Pass both arguments
		});

		builder.Services.AddAppServices();
		return builder;
	}
}
