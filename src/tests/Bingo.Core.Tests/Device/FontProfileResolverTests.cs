using Bingo.Core.Device;
using Bingo.Core.Device.Fonts;

namespace Bingo.Core.Tests.Device;

public class FontProfileResolverTests
{
	[Theory]
	[InlineData("Pixel 7", 6.3, 1080, DisplayClass.Phone, FontProfile.Phone, 28, 19)]
	[InlineData("SM-T220", 8.7, 1340, DisplayClass.CompactTablet, FontProfile.Tablet, 30, 28)]
	[InlineData("Pixel Tablet", 10.9, 1600, DisplayClass.FullTablet, FontProfile.TabletXL, 54, 48)]
	[InlineData("Windows 11 PC", 13.5, 2560, DisplayClass.Desktop, FontProfile.Windows, 64, 56)]
	[InlineData("ProjectorX", 14.5, 2048, DisplayClass.Projector, FontProfile.TabletXL, 64, 58)]
	public void Resolve_ShouldReturnExpectedProfileAndFontSizes(
		string model,
		double diagonalInches,
		double screenWidthDp,
		DisplayClass displayClass,
		FontProfile expectedProfile,
		double expectedHeaderSize,
		double expectedNumberSize)
	{
		DevicePersona persona = new()
		{
			DeviceModel = model,
			DiagonalInches = diagonalInches,
			ScreenWidthInDp = screenWidthDp,
			DisplayClass = displayClass,
			FormFactor = DeviceFormFactor.Tablet // Can vary per test case
		};

		FontProfileResult result = FontProfileResolver.Resolve(persona);

		Assert.Equal(expectedProfile, result.Profile);
		Assert.Equal(displayClass, result.DisplayClass);
		Assert.Equal(expectedHeaderSize, result.FontSet.Header.Size);
		Assert.Equal(expectedNumberSize, result.FontSet.Number.Size);
	}
}
