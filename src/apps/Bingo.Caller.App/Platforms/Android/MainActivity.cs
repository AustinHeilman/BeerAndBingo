using Android.App;
using Android.Content.PM;

namespace Bingo.Caller.App
{
	[Activity(
		Theme = "@style/Maui.SplashTheme",
		MainLauncher = true,
		LaunchMode = LaunchMode.SingleTop,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
		ScreenOrientation = ScreenOrientation.Landscape // Force landscape mode
	)]
	public class MainActivity : MauiAppCompatActivity
	{
	}
}
