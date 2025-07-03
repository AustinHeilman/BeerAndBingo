using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Bingo.UI.Shared;

[Activity(
	Label = "Bingo",
	Theme = "@style/Maui.SplashTheme",
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Keep the screen awake during gameplay
		Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
	}
}
