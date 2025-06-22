namespace Bingo.Caller.App.Startup;

public static class AppResourceExtensions
{
    public static void AddCustomResources(this App app)
    {
        // Add the resource dictionary to the application's merged dictionaries
        Application.Current?.Resources.MergedDictionaries.Add(new Bingo.UI.Shared.Styles.FlashBoardTheme());        
    }
}
