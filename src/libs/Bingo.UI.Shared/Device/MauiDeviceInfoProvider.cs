using Bingo.Core.Device;

public class MauiDeviceInfoProvider : IDeviceInfoProvider
{
	public DisplayClass DisplayClass => ResolveDisplayClass();
	public DeviceFormFactor FormFactor
	{
		get
		{
			DeviceIdiom idiom = DeviceInfo.Idiom;

			if (idiom == DeviceIdiom.Tablet)
				return DeviceFormFactor.Tablet;
			if (idiom == DeviceIdiom.Phone)
				return DeviceFormFactor.Phone;
			if (idiom == DeviceIdiom.Desktop)
				return DeviceFormFactor.Desktop;

			return DeviceFormFactor.Unknown;
		}
	}

	public DevicePersona Persona => new()
	{
		DeviceModel = DeviceModel,
		DisplayClass = DisplayClass,
		FormFactor = FormFactor,
		ScreenWidthInDp = ScreenWidthInDp,
		ScreenHeightInDp = ScreenHeightInDp,
		DiagonalInches = ScreenDiagonalInches
	};

	public double ScreenWidthInDp => DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
	public double ScreenHeightInDp => DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
	public double ScreenDiagonalInches => CalculateDiagonalInInches();
	public string DeviceModel => DeviceInfo.Model;

	private DisplayClass ResolveDisplayClass()
	{
		double width = ScreenWidthInDp;
		double height = ScreenHeightInDp;
		double diagonal = ScreenDiagonalInches;

		if (DeviceModel.Contains("A7 Lite", StringComparison.OrdinalIgnoreCase))
			return DisplayClass.CompactTablet;
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
			return DisplayClass.Desktop;
		if (diagonal >= 13)
			return DisplayClass.Projector;
		if (FormFactor == DeviceFormFactor.Tablet && diagonal >= 11)
			return DisplayClass.FullTablet;
		if (FormFactor == DeviceFormFactor.Tablet || width >= 1024)
			return DisplayClass.CompactTablet;
		if (diagonal < 5.5)
			return DisplayClass.UltraCompact;

		return DisplayClass.Phone;
	}

	private double CalculateDiagonalInInches()
	{
		double widthInPixels = DeviceDisplay.MainDisplayInfo.Width;
		double heightInPixels = DeviceDisplay.MainDisplayInfo.Height;
		double density = DeviceDisplay.MainDisplayInfo.Density;

		double widthInInches = widthInPixels / (density * 160.0);
		double heightInInches = heightInPixels / (density * 160.0);
		return Math.Sqrt(widthInInches * widthInInches + heightInInches * heightInInches);
	}
}
