namespace Bingo.UI.Shared.Extensions;

using CoreFontWeight = Bingo.Core.Device.Fonts.FontWeight;

public static class FontWeightMapper
{
	public static FontAttributes ToFontAttributes(this CoreFontWeight weight) =>
		weight switch
		{
			CoreFontWeight.Bold => FontAttributes.Bold,
			CoreFontWeight.Italic => FontAttributes.Italic,
			_ => FontAttributes.None
		};
}