using Microsoft.Extensions.DependencyInjection;
using Bingo.Services.Patterns;
using Bingo.AppServices.Patterns;
using Bingo.AppServices.Feedback;
using Bingo.AppServices.Recognition;
using Bingo.Services.Feedback;
using Bingo.Services.Recognition;

namespace Bingo.AppServices.Configuration;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IPatternService, PatternService>();
        services.AddSingleton<IWinningPatternEvaluator, WinningPatternEvaluator>();
        services.AddSingleton<IPatternRepository, DefaultPatternRepository>();

        services.AddSingleton<IMascotFeedbackService, MascotFeedbackService>();
        services.AddSingleton<ICardRecognitionService, CardRecognitionService>();

        return services;
    }
}
