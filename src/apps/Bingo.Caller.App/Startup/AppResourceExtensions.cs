using Bingo.Core.Device;
using Bingo.UI.Shared.Device;
using Bingo.UI.Shared.Services;
using Bingo.UI.Shared.Styles;

namespace Bingo.Caller.App.Startup;

public static class AppResourceExtensions
{
    public static void AddCustomResources(this App app)
    {
        // Ensure any existing resources are cleared before registering new ones
        app.Resources.MergedDictionaries.Clear();

        // Load shared theme styles
        app.Resources.MergedDictionaries.Add(new FlashBoardTheme());
             

        // Dynamically scaled typography based on screen size + platform
        MauiDeviceInfoProvider deviceInfoProvider = new();
        FontStyleService fontStyleService = new(deviceInfoProvider);
        var result = fontStyleService.GetFontProfile();
        var profile = result.Profile;
        var fontSet = result.FontSet;

        app.Resources["FlashBoardFontSet"] = fontSet;
        app.Resources["FlashBoardFontProfile"] = profile;

        // Store font set as a global resource
        app.Resources["FlashBoardFontSet"] = fontSet;

        // Store individual styles for direct referencing if needed
        app.Resources["FlashBoardHeaderFontStyle"] = fontSet.Header;
        app.Resources["FlashBoardNumberFontStyle"] = fontSet.Number;
        app.Resources["FlashBoardButtonFontStyle"] = fontSet.Button;
        app.Resources["FlashBoardLabelFontStyle"] = fontSet.Label;

        app.Resources["FlashBoardNumberFontSize"] = fontSet.Number.Size;
        app.Resources["FlashBoardNumberFontFamily"] = fontSet.Number.FontFamily;
        app.Resources["FlashBoardNumberFontAttributes"] = fontSet.Number.Weight;

        app.Resources["FlashBoardHeaderFontSize"] = fontSet.Header.Size;
        app.Resources["FlashBoardHeaderFontFamily"] = fontSet.Header.FontFamily;
        app.Resources["FlashBoardHeaderFontAttributes"] = fontSet.Header.Weight;
    }
}
