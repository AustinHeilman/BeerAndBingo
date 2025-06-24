using Bingo.Core.Device;

namespace Bingo.UI.Shared.Device;

public class MauiDeviceInfoProvider : IDeviceInfoProvider
{
    private const double BaselineDpi = 160.0;
    private readonly DisplayInfo _displayInfo;

    public MauiDeviceInfoProvider()
    {
        _displayInfo = DeviceDisplay.MainDisplayInfo;
    }

    public DeviceFormFactor FormFactor
    {
        get
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                return DeviceFormFactor.Phone;
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
                return DeviceFormFactor.Tablet;
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
                return DeviceFormFactor.Desktop;
            return DeviceFormFactor.Unknown;
        }
    }

    public double ScreenWidthInDp => _displayInfo.Width / _displayInfo.Density;
    public double ScreenHeightInDp => _displayInfo.Height / _displayInfo.Density;

    public double ScreenDiagonalInInches
    {
        get
        {
            double widthInInches = ScreenWidthInDp / BaselineDpi;
            double heightInInches = ScreenHeightInDp / BaselineDpi;
            return Math.Sqrt(widthInInches * widthInInches + heightInInches * heightInInches);
        }
    }
}
