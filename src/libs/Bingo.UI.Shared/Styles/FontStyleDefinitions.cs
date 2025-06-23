namespace Bingo.UI.Shared.Styles;

public record FontStyle(double Size, FontAttributes Attributes, string FontFamily);

public record FontSet(
    FontStyle Header,
    FontStyle Number,
    FontStyle Button,
    FontStyle Label
);
