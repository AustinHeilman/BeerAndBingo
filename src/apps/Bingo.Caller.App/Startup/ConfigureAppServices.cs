using Bingo.AppServices.Configuration;
using Bingo.UI.Shared.Services;
using Bingo.UI.Shared.Views.FlashBoard;
using Bingo.ViewModel.MainPage.Caller;

namespace Bingo.Caller.App.Startup;

public static class ConfigureAppServices
{
	public static MauiAppBuilder AddBeerAndBingoServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<IDeviceInfoProvider, MauiDeviceInfoProvider>();
		builder.Services.AddSingleton<FontStyleService>();
		builder.Services.AddTransient<FlashBoardView>();
		builder.Services.AddTransient<CallerMainPageViewModel>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddSingleton<StyleBindingService>();

		builder.Services.AddAppServices();
		return builder;
	}
}
