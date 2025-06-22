using Bingo.Core.Device;

public interface IDeviceInfoProvider
{
    DeviceFormFactor FormFactor { get; }
    double ScreenWidthInDp { get; }
    double ScreenHeightInDp { get; }
    double ScreenDiagonalInInches { get; }
}
