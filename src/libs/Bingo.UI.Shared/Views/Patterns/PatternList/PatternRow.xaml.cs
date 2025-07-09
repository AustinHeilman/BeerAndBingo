using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;

namespace Bingo.UI.Shared.Views.Patterns.PatternList;

public partial class PatternRow : ContentView
{
	public PatternRow()
	{
		InitializeComponent();
		this.BindingContext = this;
	}


	public static readonly BindableProperty PatternProperty =
		BindableProperty.Create(nameof(Pattern), typeof(BingoPattern), typeof(PatternRow), null);

	public BingoPattern Pattern
	{
		get => (BingoPattern)GetValue(PatternProperty);
		set => SetValue(PatternProperty, value);
	}

	public static readonly BindableProperty SelectedPatternProperty =
		BindableProperty.Create(nameof(SelectedPattern), typeof(BingoPattern), typeof(PatternRow), null);

	public BingoPattern SelectedPattern
	{
		get => (BingoPattern)GetValue(SelectedPatternProperty);
		set => SetValue(SelectedPatternProperty, value);
	}

	private void OnTapped(object sender, EventArgs e)
	{
		if (Pattern is null) return;

		// Look for the view model by walking the visual tree
		var vm = this.FindParent<LoadPatternView>()?.BindingContext as LoadPatternViewModel;
		if (vm is not null)
			vm.SelectedPattern = Pattern;
	}

	private T? FindParent<T>() where T : Element
	{
		Element parent = this.Parent;
		while (parent is not null && parent is not T)
			parent = parent.Parent;
		return parent as T;
	}
}
