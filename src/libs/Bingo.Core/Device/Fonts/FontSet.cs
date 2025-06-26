namespace Bingo.Core.Device.Fonts;

public record FontSet(
	FontStyle Header,
	FontStyle Number,
	FontStyle Button,
	FontStyle Label,
	FontStyle MicroLabel // for ghosted, embedded, or secondary glyphs
);