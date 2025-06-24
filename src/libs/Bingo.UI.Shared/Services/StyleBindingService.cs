using Bingo.UI.Shared.Styles;

namespace Bingo.UI.Shared.Services;

public class StyleBindingService
{
    private readonly FontStyleService _fontStyleService;

    public StyleBindingService(FontStyleService fontStyleService)
    {
        _fontStyleService = fontStyleService;
    }

    public FontSet GetFontSet() => _fontStyleService.GetFontSet();

    public Label CreateStyledLabel(FontStyle style, string text, Color? textColor = null)
    {
        Color? flashCellTextColor = Application.Current?.Resources?["FlashCellTextColor"] as Color;

        return new Label
        {
            Text = text,
            FontSize = style.Size,
            FontAttributes = style.Attributes,
            FontFamily = style.FontFamily,
            TextColor = textColor ?? flashCellTextColor ?? Colors.Black, // Fallback to Colors.Black if null
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.NoWrap,
            Padding = new Thickness(4)
        };
    }
}
