using Bingo.Core.Device;
using Bingo.UI.Shared.Styles;

public class FontStyleService
{
    private readonly IDeviceInfoProvider _deviceInfo;

    public FontStyleService(IDeviceInfoProvider deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }

    public FontSet GetFontSet()
    {
        var isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;
        var isTablet = _deviceInfo.FormFactor == DeviceFormFactor.Tablet;
        var inches = _deviceInfo.ScreenDiagonalInInches;

        if (isWindows)
        {
            return new FontSet(
                new(64, FontAttributes.Bold, "Consolas"),
                new(56, FontAttributes.Bold, "Consolas"),
                new(42, FontAttributes.None, "Segoe UI"),
                new(28, FontAttributes.None, "Segoe UI")
            );
        }

        if (isTablet)
        {
            return inches switch
            {
                >= 11 => new FontSet(
                    new(54, FontAttributes.Bold, "Consolas"),
                    new(48, FontAttributes.Bold, "Consolas"),
                    new(34, FontAttributes.None, "Roboto"),
                    new(24, FontAttributes.None, "Roboto")
                ),
                _ => new FontSet(
                    new(48, FontAttributes.Bold, "Consolas"),
                    new(42, FontAttributes.Bold, "Consolas"),
                    new(30, FontAttributes.None, "Roboto"),
                    new(20, FontAttributes.None, "Roboto")
                )
            };
        }

        return new FontSet(
            new(40, FontAttributes.Bold, "Consolas"),
            new(34, FontAttributes.Bold, "Consolas"),
            new(26, FontAttributes.None, "Roboto"),
            new(18, FontAttributes.None, "Roboto")
        );
    }
}
