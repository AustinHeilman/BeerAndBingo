// FontSizeService.cs
using Bingo.Core.Device;

namespace Bingo.UI.Shared.Device;
public class FontSizeService
{
    private readonly IDeviceInfoProvider _info;

    public FontSizeService(IDeviceInfoProvider info)
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
        return _info.FormFactor switch
        {
            DeviceFormFactor.Desktop or DeviceFormFactor.Tablet when _info.ScreenDiagonalInInches > 10 =>
                new(40, 36, 28, 22),

            DeviceFormFactor.Tablet =>
                new(34, 30, 24, 18),

            DeviceFormFactor.Phone =>
                new(26, 24, 20, 16),

            _ =>
                new(24, 22, 18, 14)
        };
    }
}
