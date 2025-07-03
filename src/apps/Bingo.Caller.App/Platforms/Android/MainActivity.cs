using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Bingo.Caller.App;

[Activity(
	Label = "Beer&Bingo - Caller",
	Theme = "@style/Maui.SplashTheme",
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		// No need to set the flag here
	}

	protected override void OnResume()
	{
		base.OnResume();
		// Keep the screen awake while the app is active
		Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
	}

	protected override void OnPause()
	{
		// Allow the screen to turn off when the app is not active
		Window?.ClearFlags(WindowManagerFlags.KeepScreenOn);
		base.OnPause();
	}
}
