using Bingo.AppServices.Feedback;
using Bingo.AppServices.Recognition;
using Bingo.Services.Feedback;
using Bingo.Services.Recognition;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.AppServices.Configuration;

public static class AppServiceRegistration
{
	public static IServiceCollection AddAppServices(this IServiceCollection services)
	{
		services.AddSingleton<IMascotFeedbackService, MascotFeedbackService>();
		services.AddSingleton<ICardRecognitionService, CardRecognitionService>();

		return services;
	}
}
