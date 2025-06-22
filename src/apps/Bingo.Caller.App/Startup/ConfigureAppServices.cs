using Bingo.AppServices.Configuration;

namespace Bingo.Caller.App.Startup;

public static class ConfigureAppServices
{
    public static MauiAppBuilder AddBeerAndBingoServices(this MauiAppBuilder builder)
    {
        builder.Services.AddAppServices();
        return builder;
    }
}
