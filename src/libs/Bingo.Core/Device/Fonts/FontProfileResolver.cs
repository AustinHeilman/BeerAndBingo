using Bingo.Core.Device;

namespace Bingo.Core.Device.Fonts;

public static class FontProfileResolver
{
    public static FontProfileResult Resolve(DevicePersona persona)
    {
        return persona.DisplayClass switch
        {
            DisplayClass.UltraCompact => new(
                FontProfile.CompactPhone,
                new FontSet(
                    new(30, FontWeight.Bold, "Consolas"),
                    new(19, FontWeight.Bold, "Consolas"),
                    new(18, FontWeight.Normal, "Roboto"),
                    new(14, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),

            DisplayClass.Phone => new(
                FontProfile.Phone,
                new FontSet(
                    new(28, FontWeight.Bold, "Consolas"),
                    new(19, FontWeight.Bold, "Consolas"),
                    new(14, FontWeight.Normal, "Roboto"),
                    new(14, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),

            DisplayClass.CompactTablet => new(
                FontProfile.Tablet,
                new FontSet(
                    new(30, FontWeight.Bold, "Consolas"),
                    new(28, FontWeight.Bold, "Consolas"),
                    new(14, FontWeight.Normal, "Roboto"),
                    new(14, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),

            DisplayClass.FullTablet => new(
                FontProfile.TabletXL,
                new FontSet(
                    new(54, FontWeight.Bold, "Consolas"),
                    new(48, FontWeight.Bold, "Consolas"),
                    new(34, FontWeight.Normal, "Roboto"),
                    new(24, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),

            DisplayClass.Desktop => new(
                FontProfile.Windows,
                new FontSet(
                    new(64, FontWeight.Bold, "Consolas"),
                    new(56, FontWeight.Bold, "Consolas"),
                    new(42, FontWeight.Normal, "Segoe UI"),
                    new(28, FontWeight.Normal, "Segoe UI")
                ),
                persona.DisplayClass),

            DisplayClass.Projector => new(
                FontProfile.TabletXL,
                new FontSet(
                    new(64, FontWeight.Bold, "Consolas"),
                    new(58, FontWeight.Bold, "Consolas"),
                    new(36, FontWeight.Normal, "Roboto"),
                    new(26, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),

            _ => new(
                FontProfile.Phone,
                new FontSet(
                    new(28, FontWeight.Bold, "Consolas"),
                    new(19, FontWeight.Bold, "Consolas"),
                    new(14, FontWeight.Normal, "Roboto"),
                    new(14, FontWeight.Normal, "Roboto")
                ),
                persona.DisplayClass),
        };
    }
}
