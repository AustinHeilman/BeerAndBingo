using Bingo.Core.Device.Fonts;
using Bingo.UI.Shared.Extensions;
using Bingo.UI.Shared.Styles;

namespace Bingo.Caller.App.Startup;

public static class AppResourceExtensions
{
	public static void AddCustomResources(this App app)
	{
		// Clear any stale resource dictionaries
		app.Resources.MergedDictionaries.Clear();

		// Load shared theme styles
		app.Resources.MergedDictionaries.Add(new FlashBoardTheme());

		// Resolve persona + fonts
		MauiDeviceInfoProvider deviceInfoProvider = new();
		DevicePersona persona = deviceInfoProvider.Persona;
		FontProfileResult result = FontProfileResolver.Resolve(persona);
		FontProfile profile = result.Profile;
		FontSet fontSet = result.FontSet;

		// Register global font resources
		app.Resources["FlashBoardFontProfile"] = profile;
		app.Resources["FlashBoardFontSet"] = fontSet;

		app.Resources["FlashBoardHeaderFontStyle"] = fontSet.Header;
		app.Resources["FlashBoardNumberFontStyle"] = fontSet.Number;
		app.Resources["FlashBoardButtonFontStyle"] = fontSet.Button;
		app.Resources["FlashBoardLabelFontStyle"] = fontSet.Label;
		app.Resources["FlashBoardMicroLabelFontStyle"] = fontSet.MicroLabel;

		app.Resources["FlashBoardHeaderFontSize"] = fontSet.Header.Size;
		app.Resources["FlashBoardHeaderFontFamily"] = fontSet.Header.FontFamily;
		app.Resources["FlashBoardHeaderFontAttributes"] = fontSet.Header.Weight.ToFontAttributes();

		app.Resources["FlashBoardNumberFontSize"] = fontSet.Number.Size;
		app.Resources["FlashBoardNumberFontFamily"] = fontSet.Number.FontFamily;
		app.Resources["FlashBoardNumberFontAttributes"] = fontSet.Number.Weight.ToFontAttributes();

		// Bonus: register font attributes for any other roles
		app.Resources["FlashBoardButtonFontSize"] = fontSet.Button.Size;
		app.Resources["FlashBoardLabelFontSize"] = fontSet.Label.Size;
		app.Resources["FlashBoardMicroLabelFontSize"] = fontSet.MicroLabel.Size;
	}
}
