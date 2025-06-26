namespace Bingo.UI.Shared.Helpers;

public static class PlatformIconHelper
{
	public static ImageSource Get(string baseName)
	{
#if WINDOWS
		return ImageSource.FromFile($"{baseName}_win.png");
#else
		return ImageSource.FromFile($"{baseName}.svg");
#endif
	}
}
