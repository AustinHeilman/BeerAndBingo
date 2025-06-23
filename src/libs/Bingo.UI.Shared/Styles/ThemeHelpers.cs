namespace Bingo.UI.Shared.Styles;

public static class ThemeHelpers
{
    public static Color GetAppColor(string key, Color fallback)
    {
        return Application.Current?.Resources.TryGetValue(key, out var value) == true
            ? (Color)value
            : fallback;
    }
}