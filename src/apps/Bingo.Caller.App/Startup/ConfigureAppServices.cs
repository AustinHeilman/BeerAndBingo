using Bingo.AppServices.Configuration;
using Bingo.UI.Shared.Device;
using Bingo.UI.Shared.Views.FlashBoard;

namespace Bingo.Caller.App.Startup;

public static class ConfigureAppServices
{
    public static MauiAppBuilder AddBeerAndBingoServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDeviceInfoProvider, MauiDeviceInfoProvider>();
        builder.Services.AddSingleton<FontSizeService>();
        
        builder.Services.AddTransient<FlashBoardView>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddAppServices();
        return builder;
    }
}
