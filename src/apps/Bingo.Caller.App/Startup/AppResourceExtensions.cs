using Bingo.UI.Shared.Styles;

namespace Bingo.Caller.App.Startup;

public static class AppResourceExtensions
{
    public static void AddCustomResources(this App app)
    {
        app.Resources.MergedDictionaries.Clear();
        app.Resources.MergedDictionaries.Add(new FlashBoardTheme());

        app.Resources["FlashCellTextColor"] = Colors.Black;
        app.Resources["FlashHeaderTextColor"] = Colors.LightYellow;
        app.Resources["FlashCellBGColor_Uncalled"] = Colors.DarkGray;
        app.Resources["FlashCellBGColor_Called"] = Colors.Goldenrod;
    }

}
