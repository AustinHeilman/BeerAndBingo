using Bingo.UI.Shared.Styles;
using Bingo.Core.Device;
using Microsoft.Maui.Controls;
using Bingo.UI.Shared.Device;

namespace Bingo.Caller.App.Startup;

public static class AppResourceExtensions
{
    public static void AddCustomResources(this App app)
    {
        // Ensure any existing resources are cleared before registering new ones
        app.Resources.MergedDictionaries.Clear();

        // Load shared theme styles
        app.Resources.MergedDictionaries.Add(new FlashBoardTheme());
        
        // Runtime-generated color resources
        app.Resources["FlashCellTextColor"] = Colors.Black;
        app.Resources["FlashHeaderTextColor"] = Colors.LightYellow;
        app.Resources["FlashCellBGColor_Uncalled"] = Colors.DarkGray;
        app.Resources["FlashCellBGColor_Called"] = Colors.Goldenrod;

        // Dynamically scaled typography based on screen size + platform
        var deviceInfoProvider = new MauiDeviceInfoProvider();
        var fontService = new FontStyleService(deviceInfoProvider);
        var fontSet = fontService.GetFontSet();

        // Store font set as a global resource
        app.Resources["FlashBoardFontSet"] = fontSet;

        // Store individual styles for direct referencing if needed
        app.Resources["FlashBoardHeaderFontStyle"] = fontSet.Header;
        app.Resources["FlashBoardNumberFontStyle"] = fontSet.Number;
        app.Resources["FlashBoardButtonFontStyle"] = fontSet.Button;
        app.Resources["FlashBoardLabelFontStyle"] = fontSet.Label;
    }
}
