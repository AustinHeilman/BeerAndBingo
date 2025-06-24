using Bingo.Core.Device;

public class DevicePersona
{
    public string DeviceModel { get; init; } = string.Empty;
    public DisplayClass DisplayClass { get; init; }
    public DeviceFormFactor FormFactor { get; init; }
    public double ScreenWidthInDp { get; init; }
    public double ScreenHeightInDp { get; init; }
    public double DiagonalInches { get; init; }

    public bool IsLowDpi => (ScreenWidthInDp < 1340 && DiagonalInches < 9);
    public bool IsCompactTablet => DisplayClass == DisplayClass.CompactTablet;
    public bool IsProjector => DisplayClass == DisplayClass.Projector;
    public bool IsSmallPhone => DisplayClass == DisplayClass.UltraCompact;
    public bool IsA7Lite => DeviceModel.Contains("A7 Lite", StringComparison.OrdinalIgnoreCase);
}