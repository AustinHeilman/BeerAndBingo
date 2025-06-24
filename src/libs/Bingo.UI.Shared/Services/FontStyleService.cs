using Bingo.Core.Device;
using Bingo.UI.Shared.Styles;
using System.Diagnostics;

namespace Bingo.UI.Shared.Services;

public class FontStyleService
{
    private readonly IDeviceInfoProvider _deviceInfo;

    public FontStyleService(IDeviceInfoProvider deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }

    public (FontProfile Profile, FontSet FontSet) GetFontProfile()
    {
        bool isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;
        bool isTablet = _deviceInfo.FormFactor == DeviceFormFactor.Tablet;
        double inches = _deviceInfo.ScreenDiagonalInInches;

        if (isWindows)
        {
            return (FontProfile.Windows, new FontSet(
                new(64, FontAttributes.Bold, "Consolas"),
                new(56, FontAttributes.Bold, "Consolas"),
                new(42, FontAttributes.None, "Segoe UI"),
                new(28, FontAttributes.None, "Segoe UI")
            ));
        }

        if (isTablet)
        {   
            Debug.WriteLine($"Tablet detected with diagonal inches: {inches}");
            if (inches >= 11)
            {
                return (FontProfile.TabletXL, new FontSet(
                    new(54, FontAttributes.Bold, "Consolas"),
                    new(48, FontAttributes.Bold, "Consolas"),
                    new(34, FontAttributes.None, "Roboto"),
                    new(24, FontAttributes.None, "Roboto")
                ));
            }
            else if ( inches >= 7.0)
            {
                return (FontProfile.Tablet, new FontSet(
                    new(38, FontAttributes.Bold, "Consolas"),
                    new(30, FontAttributes.Bold, "Consolas"),
                    new(28, FontAttributes.None, "Roboto"),
                    new(18, FontAttributes.None, "Roboto")
                ));
            }
            else
            {
                return (FontProfile.Tablet, new FontSet(
                    new(48, FontAttributes.Bold, "Consolas"),
                    new(42, FontAttributes.Bold, "Consolas"),
                    new(30, FontAttributes.None, "Roboto"),
                    new(20, FontAttributes.None, "Roboto")
                ));
            }
        }

        if (inches < 6.4)
        {
            return (FontProfile.CompactPhone, new FontSet(
                new(30, FontAttributes.Bold, "Consolas"),
                new(19, FontAttributes.Bold, "Consolas"),
                new(18, FontAttributes.None, "Roboto"),
                new(14, FontAttributes.None, "Roboto")
            ));
        }

        return (FontProfile.Phone, new FontSet(
            new(40, FontAttributes.Bold, "Consolas"),
            new(34, FontAttributes.Bold, "Consolas"),
            new(26, FontAttributes.None, "Roboto"),
            new(18, FontAttributes.None, "Roboto")
        ));
    }
}
