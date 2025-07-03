using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Bingo.Caller.App;

[Activity(
	Label = "Beer&Bingo - Caller",
	Theme = "@style/Maui.SplashTheme",
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
	ScreenOrientation = ScreenOrientation.Landscape)] // Force landscape
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Enable full screen (hide status and navigation bars)
#pragma warning disable CA1416 // Suppress platform dependent API warning
		Window?.AddFlags(WindowManagerFlags.Fullscreen);

		if (Window != null && Window.DecorView != null)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.R) // Android 11 (API 30) and above
			{
				IWindowInsetsController? controller = Window.InsetsController;
				if (controller != null)
				{
					controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());
					controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
				}
			}
			else
			{
#pragma warning disable CA1422 // Suppress platform dependent API warning
				Window.DecorView.SystemUiFlags =
					SystemUiFlags.ImmersiveSticky
					| SystemUiFlags.HideNavigation
					| SystemUiFlags.Fullscreen;
#pragma warning restore CA1422
			}
		}
#pragma warning restore CA1416
	}

	protected override void OnResume()
	{
		base.OnResume();
		Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
	}

	protected override void OnPause()
	{
		Window?.ClearFlags(WindowManagerFlags.KeepScreenOn);
		base.OnPause();
	}
}
