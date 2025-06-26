using Bingo.Core.Device;
using Bingo.Core.Device.Fonts;
using Bingo.UI.Shared.Extensions;

namespace Bingo.UI.Shared.Services;

public class StyleBindingService
{
	private readonly FontSet _fontSet;
	private readonly FontProfile _fontProfile;

	public StyleBindingService(IDeviceInfoProvider deviceInfoProvider)
	{
		var persona = deviceInfoProvider.Persona;
		FontProfileResult result = FontProfileResolver.Resolve(persona);
		_fontSet = result.FontSet;
		_fontProfile = result.Profile;
	}

	public FontSet GetFontSet() => _fontSet;

	public Label CreateStyledLabel(FontStyle style, string text, Color? textColor = null)
	{
		Color? themedColor = Application.Current?.Resources?["FlashCellTextColor"] as Color;

		return new Label
		{
			Text = text,
			FontSize = style.Size,
			FontAttributes = style.Weight.ToFontAttributes(),
			FontFamily = style.FontFamily,
			TextColor = textColor ?? themedColor ?? Colors.Black,
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			LineBreakMode = LineBreakMode.NoWrap,
			Padding = new Thickness(4)
		};
	}
}
