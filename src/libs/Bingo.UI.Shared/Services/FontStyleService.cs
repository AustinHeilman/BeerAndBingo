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
        var persona = _deviceInfo.Persona;

        FontProfile profile;
        FontSet fontSet;

        switch (persona.DisplayClass)
        {
            case DisplayClass.Desktop:
                profile = FontProfile.Windows;
                fontSet = new FontSet(
                    new(64, FontAttributes.Bold, "Consolas"),
                    new(56, FontAttributes.Bold, "Consolas"),
                    new(42, FontAttributes.None, "Segoe UI"),
                    new(28, FontAttributes.None, "Segoe UI")
                );
                break;

            case DisplayClass.Projector:
            case DisplayClass.FullTablet:
                profile = FontProfile.TabletXL;
                fontSet = new FontSet(
                    new(54, FontAttributes.Bold, "Consolas"),
                    new(48, FontAttributes.Bold, "Consolas"),
                    new(34, FontAttributes.None, "Roboto"),
                    new(24, FontAttributes.None, "Roboto")
                );
                break;

            case DisplayClass.CompactTablet:
                profile = FontProfile.Tablet;
                fontSet = new FontSet(
                    new(30, FontAttributes.Bold, "Consolas"),
                    new(28, FontAttributes.Bold, "Consolas"),
                    new(14, FontAttributes.None, "Roboto"),
                    new(14, FontAttributes.None, "Roboto")
                );
                break;

            case DisplayClass.UltraCompact:
                profile = FontProfile.CompactPhone;
                fontSet = new FontSet(
                    new(20, FontAttributes.Bold, "Consolas"),
                    new(14, FontAttributes.Bold, "Consolas"),
                    new(10, FontAttributes.None, "Roboto"),
                    new(10, FontAttributes.None, "Roboto")
                );
                break;

            default:
            case DisplayClass.Phone:
                profile = FontProfile.Phone;
                fontSet = new FontSet(
                    new(28, FontAttributes.Bold, "Consolas"),
                    new(19, FontAttributes.Bold, "Consolas"),
                    new(14, FontAttributes.None, "Roboto"),
                    new(14, FontAttributes.None, "Roboto")
                );
                break;
        }

        Debug.WriteLine($"Font profile: {profile}, display class: {persona.DisplayClass}, model: {persona.DeviceModel}");

        return new FontProfileResult(profile, fontSet, persona.DisplayClass);
    }

}
