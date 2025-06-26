using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using System.Diagnostics;

namespace Bingo.UI.Shared.Views.Patterns.PatternDisplay;

public partial class PatternDisplayView : ContentView
{
	public static readonly BindableProperty PatternCellsProperty =
		BindableProperty.Create(
			nameof(PatternCells),
			typeof(ISet<(int, int)>),
			typeof(PatternDisplayView),
			new HashSet<(int, int)>(),
			propertyChanged: (bindable, oldVal, newVal) =>
			{
				if (bindable is PatternDisplayView view && newVal is ISet<(int, int)> cells)
				{
					view.PatternCells = cells;
				}
			});

	public ISet<(int, int)> PatternCells
	{
		get => (ISet<(int, int)>)GetValue(PatternCellsProperty);
		set
		{
			if (value is null)
				return;

			SetValue(PatternCellsProperty, value);
			UpdatePatternVisuals();
		}
	}

	public static readonly BindableProperty ViewModelProperty =
		BindableProperty.Create(
			nameof(ViewModel),
			typeof(PatternDisplayViewModel),
			typeof(PatternDisplayView),
			default(PatternDisplayViewModel),
			propertyChanged: OnViewModelChanged);

	public PatternDisplayViewModel ViewModel
	{
		get => (PatternDisplayViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	// 📌 Fast-access lookup for grid performance
	private readonly Dictionary<(int Row, int Col), BoxView> _boxMap = new();

	public PatternDisplayView()
	{
		InitializeComponent();
		PatternCells = new HashSet<(int, int)>();
		BuildGrid();
	}

	private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not PatternDisplayView view || newValue is not PatternDisplayViewModel vm)
			return;

		view.BindingContext = vm;

		view.PatternCells = vm.PatternCells
			.Select(cell => (cell.Row, cell.Col))
			.ToHashSet();

		vm.PatternCells.CollectionChanged += (_, __) =>
		{
			view.PatternCells = vm.PatternCells
				.Select(cell => (cell.Row, cell.Col))
				.ToHashSet();
		};
	}

	private void BuildGrid()
	{
		int rows = PatternGridSettings.PatternRowCount;
		int cols = PatternGridSettings.PatternColCount;

		PatternGrid.RowDefinitions.Clear();
		PatternGrid.ColumnDefinitions.Clear();
		PatternGrid.Children.Clear();
		_boxMap.Clear();

		for (int r = 0; r < rows; r++)
			PatternGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

		for (int c = 0; c < cols; c++)
			PatternGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

		for (int r = 0; r < rows; r++)
		{
			for (int c = 0; c < cols; c++)
			{
				var box = new BoxView
				{
					Color = Colors.LightGray,
					BindingContext = (r, c),
					CornerRadius = 3,
					Margin = 1
				};

				PatternGrid.Children.Add(box);
				Grid.SetRow(box, r);
				Grid.SetColumn(box, c);
				_boxMap[(r, c)] = box;
			}
		}

		UpdatePatternVisuals();
	}

	private void UpdatePatternVisuals()
	{
		Color fallbackColor = (PatternCells == null || PatternCells.Count == 0)
			? Colors.Gray
			: Colors.LightGray;

		foreach (var pos in _boxMap.Keys)
		{
			if (PatternCells == null)
			{
				Debug.WriteLine($"PatternCells is null, skipping update for {pos}");
				continue;
			}
			_boxMap[pos].Color = PatternCells.Contains(pos)
				? Colors.MediumPurple
				: fallbackColor;
		}
	}
}
