using Bingo.Core.Device;
using Bingo.Core.Device.Fonts;
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
                    new(64, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(56, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(42, Core.Device.Fonts.FontWeight.Normal, "Segoe UI"),
                    new(28, Core.Device.Fonts.FontWeight.Normal, "Segoe UI")
                );
                break;

            case DisplayClass.Projector:
            case DisplayClass.FullTablet:
                profile = FontProfile.TabletXL;
                fontSet = new FontSet(
                    new(54, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(48, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(34, Core.Device.Fonts.FontWeight.Normal, "Roboto"),
                    new(24, Core.Device.Fonts.FontWeight.Normal, "Roboto")
                );
                break;

            case DisplayClass.CompactTablet:
                profile = FontProfile.Tablet;
                fontSet = new FontSet(
                    new(30, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(28, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(14, Core.Device.Fonts.FontWeight.Normal, "Roboto"),
                    new(14, Core.Device.Fonts.FontWeight.Normal, "Roboto")
                );
                break;

            case DisplayClass.UltraCompact:
                profile = FontProfile.CompactPhone;
                fontSet = new FontSet(
                    new(20, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(14, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(10, Core.Device.Fonts.FontWeight.Normal, "Roboto"),
                    new(10, Core.Device.Fonts.FontWeight.Normal, "Roboto")
                );
                break;

            default:
            case DisplayClass.Phone:
                profile = FontProfile.Phone;
                fontSet = new FontSet(
                    new(28, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(19, Core.Device.Fonts.FontWeight.Bold, "Consolas"),
                    new(14, Core.Device.Fonts.FontWeight.Normal, "Roboto"),
                    new(14, Core.Device.Fonts.FontWeight.Normal, "Roboto")
                );
                break;
        }

        Debug.WriteLine($"Font profile: {profile}, display class: {persona.DisplayClass}, model: {persona.DeviceModel}");

        return new FontProfileResult(profile, fontSet, persona.DisplayClass);
    }

}
