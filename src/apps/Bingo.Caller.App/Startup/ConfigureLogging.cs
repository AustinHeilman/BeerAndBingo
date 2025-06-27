using Microsoft.Extensions.Logging;

namespace Bingo.Caller.App.Startup;

public static class ConfigureLogging
{
	public static MauiAppBuilder AddDebugLogging(this MauiAppBuilder builder)
	{
#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder;
	}
}
