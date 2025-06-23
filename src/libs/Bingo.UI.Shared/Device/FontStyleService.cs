// FontSizeService.cs
using Bingo.Core.Device;

namespace Bingo.UI.Shared.Device;
public class FontStyleService
{
    private readonly IDeviceInfoProvider _info;

    public FontStyleService(IDeviceInfoProvider info)
    {
        _info = info;
    }

    public record FontSizes(
        double HeaderFontSize,
        double NumberFontSize,
        double ButtonFontSize,
        double LabelFontSize
    );

    public FontSizes GetFontSizes()
    {
        var isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;
        var isTablet = _info.FormFactor == DeviceFormFactor.Tablet;
        var inches = _info.ScreenDiagonalInInches;

        if (isWindows)
        {
            return inches switch
            {
                >= 13 => new(64, 56, 42, 28),  // Large Windows
                >= 11 => new(56, 48, 36, 26),  // Small Windows
                _ => new(50, 44, 34, 24)
            };
        }

        if (isTablet)
        {
            return inches switch
            {
                >= 11 => new(54, 48, 34, 24),  // Pixel Tablet or iPad Pro
                _ => new(48, 42, 30, 22)   // Mid-size Android
            };
        }

        // Phones and fallback
        return new(40, 34, 26, 18);
    }

}
