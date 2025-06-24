using Bingo.Core.Device;
using Bingo.UI.Shared.Device;
using Bingo.UI.Shared.Styles;
using Microsoft.Maui.Devices;
using System.Diagnostics;

namespace Bingo.UI.Shared.Services;

public class FontStyleService
{
    private readonly IDeviceInfoProvider _deviceInfo;

    public FontStyleService(IDeviceInfoProvider deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }

    public FontProfileResult GetFontProfile()
    {
        var info = DeviceDisplay.MainDisplayInfo;
        double inches = _deviceInfo.ScreenDiagonalInches;
        bool isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;

        DisplayClass displayClass;
        FontProfile profile;
        FontSet fontSet;

        if (isWindows)
        {
            displayClass = DisplayClass.Desktop;
            profile = FontProfile.Windows;
            fontSet = new FontSet(
                new(64, FontAttributes.Bold, "Consolas"),
                new(56, FontAttributes.Bold, "Consolas"),
                new(42, FontAttributes.None, "Segoe UI"),
                new(28, FontAttributes.None, "Segoe UI")
            );
        }
        else if (_deviceInfo.DeviceModel.Contains("A7 Lite", StringComparison.OrdinalIgnoreCase) ||
                 info.Width <= 1340 || inches < 9.0)
        {
            displayClass = DisplayClass.CompactTablet;
            profile = FontProfile.Tablet;
            fontSet = new FontSet(
                new(38, FontAttributes.Bold, "Consolas"),
                new(30, FontAttributes.Bold, "Consolas"),
                new(26, FontAttributes.None, "Roboto"),
                new(18, FontAttributes.None, "Roboto")
            );
        }
        else if (_deviceInfo.FormFactor == DeviceFormFactor.Tablet && inches >= 11)
        {
            displayClass = DisplayClass.FullTablet;
            profile = FontProfile.TabletXL;
            fontSet = new FontSet(
                new(54, FontAttributes.Bold, "Consolas"),
                new(48, FontAttributes.Bold, "Consolas"),
                new(34, FontAttributes.None, "Roboto"),
                new(24, FontAttributes.None, "Roboto")
            );
        }
        else if (_deviceInfo.FormFactor == DeviceFormFactor.Tablet)
        {
            displayClass = DisplayClass.FullTablet;
            profile = FontProfile.Tablet;
            fontSet = new FontSet(
                new(48, FontAttributes.Bold, "Consolas"),
                new(42, FontAttributes.Bold, "Consolas"),
                new(30, FontAttributes.None, "Roboto"),
                new(20, FontAttributes.None, "Roboto")
            );
        }
        else if (inches < 6.4)
        {
            displayClass = DisplayClass.UltraCompact;
            profile = FontProfile.CompactPhone;
            fontSet = new FontSet(
                new(30, FontAttributes.Bold, "Consolas"),
                new(19, FontAttributes.Bold, "Consolas"),
                new(18, FontAttributes.None, "Roboto"),
                new(14, FontAttributes.None, "Roboto")
            );
        }
        else
        {
            displayClass = DisplayClass.Phone;
            profile = FontProfile.Phone;
            fontSet = new FontSet(
                new(40, FontAttributes.Bold, "Consolas"),
                new(34, FontAttributes.Bold, "Consolas"),
                new(26, FontAttributes.None, "Roboto"),
                new(18, FontAttributes.None, "Roboto")
            );
        }

        Debug.WriteLine($"Font profile: {profile}, display class: {displayClass}, diagonal: {inches:F2}\"");

        return new FontProfileResult(profile, fontSet, displayClass);
    }
}
