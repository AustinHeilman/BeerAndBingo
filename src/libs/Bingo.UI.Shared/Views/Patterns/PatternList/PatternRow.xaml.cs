using Bingo.Core.Patterns;

namespace Bingo.UI.Shared.Views.Patterns.PatternList;

public partial class PatternRow : ContentView
{
	public PatternRow() => InitializeComponent();

	public static readonly BindableProperty SelectedPatternProperty =
		BindableProperty.Create(nameof(SelectedPattern), typeof(BingoPattern), typeof(PatternRow), default(BingoPattern));

	public BingoPattern SelectedPattern
	{
		get => (BingoPattern)GetValue(SelectedPatternProperty);
		set => SetValue(SelectedPatternProperty, value);
	}
}
