using Bingo.Core.Device;
using Bingo.UI.Shared.Styles;

namespace Bingo.UI.Shared.Services;

public class StyleBindingService
{
    private readonly FontSet _fontSet;
    private readonly FontProfile _fontProfile;

    public StyleBindingService(FontStyleService fontStyleService)
    {
        var result = fontStyleService.GetFontProfile();
        _fontSet = result.FontSet;
        _fontProfile = result.Profile;
    }

    public FontSet GetFontSet() => _fontSet;

    public Label CreateStyledLabel(FontStyle style, string text, Color? textColor = null)
    {
        Color? flashCellTextColor = Application.Current?.Resources?["FlashCellTextColor"] as Color;

        return new Label
        {
            Text = text,
            FontSize = style.Size,
            FontAttributes = style.Attributes,
            FontFamily = style.FontFamily,
            TextColor = textColor ?? flashCellTextColor ?? Colors.Black,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.NoWrap,
            Padding = new Thickness(4)
        };
    }
}
