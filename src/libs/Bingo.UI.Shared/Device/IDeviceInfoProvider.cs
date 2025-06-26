using Bingo.Core.Device;

namespace Bingo.UI.Shared.Device;

public interface IDeviceInfoProvider
{
	DisplayClass DisplayClass { get; }
	DeviceFormFactor FormFactor { get; }
	double ScreenWidthInDp { get; }
	double ScreenHeightInDp { get; }
	double ScreenDiagonalInches { get; }
	string DeviceModel { get; }

	DevicePersona Persona { get; }
}
