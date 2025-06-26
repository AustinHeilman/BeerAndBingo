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
					new(30, FontWeight.Bold, "Inconsolata"),
					new(19, FontWeight.Bold, "Inconsolata"),
					new(18, FontWeight.Normal, "Roboto"),
					new(14, FontWeight.Normal, "Roboto"),
					new(12, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			DisplayClass.Phone => new(
				FontProfile.Phone,
				new FontSet(
					new(28, FontWeight.Bold, "Inconsolata"),
					new(19, FontWeight.Bold, "Inconsolata"),
					new(14, FontWeight.Normal, "Roboto"),
					new(14, FontWeight.Normal, "Roboto"),
					new(12, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			DisplayClass.CompactTablet => new(
				FontProfile.Tablet,
				new FontSet(
					new(30, FontWeight.Bold, "Inconsolata"),
					new(28, FontWeight.Bold, "Inconsolata"),
					new(14, FontWeight.Normal, "Roboto"),
					new(14, FontWeight.Normal, "Roboto"),
					new(12, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			DisplayClass.FullTablet => new(
				FontProfile.TabletXL,
				new FontSet(
					new(54, FontWeight.Bold, "Inconsolata"),
					new(48, FontWeight.Bold, "Inconsolata"),
					new(34, FontWeight.Normal, "Roboto"),
					new(24, FontWeight.Normal, "Roboto"),
					new(18, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			DisplayClass.Desktop => new(
				FontProfile.Windows,
				new FontSet(
					new(64, FontWeight.Bold, "Inconsolata"),
					new(56, FontWeight.Bold, "Inconsolata"),
					new(42, FontWeight.Normal, "Segoe UI"),
					new(28, FontWeight.Normal, "Segoe UI"),
					new(20, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			DisplayClass.Projector => new(
				FontProfile.TabletXL,
				new FontSet(
					new(64, FontWeight.Bold, "Inconsolata"),
					new(58, FontWeight.Bold, "Inconsolata"),
					new(36, FontWeight.Normal, "Roboto"),
					new(26, FontWeight.Normal, "Roboto"),
					new(20, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),

			_ => new(
				FontProfile.Phone,
				new FontSet(
					new(28, FontWeight.Bold, "Inconsolata"),
					new(19, FontWeight.Bold, "Inconsolata"),
					new(14, FontWeight.Normal, "Roboto"),
					new(14, FontWeight.Normal, "Roboto"),
					new(10, FontWeight.Normal, "Roboto")
				),
				persona.DisplayClass),
		};
	}
}
