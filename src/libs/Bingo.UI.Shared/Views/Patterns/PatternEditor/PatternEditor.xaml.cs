using Bingo.Core.Patterns;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class PatternEditor : ContentView
{
	public static readonly BindableProperty PatternLayoutProperty =
		BindableProperty.Create(
			nameof(PatternLayout),
			typeof(IEnumerable<PatternCell>),
			typeof(PatternEditor),
			defaultValue: Enumerable.Empty<PatternCell>(),
			propertyChanged: (bindable, oldVal, newVal) =>
			{
				if (bindable is PatternEditor view && newVal is IEnumerable<PatternCell> cells)
				{
					view.SetPatternCells(cells);
				}
			});

	public IEnumerable<PatternCell> PatternLayout
	{
		get => (IEnumerable<PatternCell>)GetValue(PatternLayoutProperty);
		set => SetValue(PatternLayoutProperty, value);
	}

	private readonly Dictionary<(int, int), Border> _borderMap = new();
	private readonly Dictionary<(int, int), PatternCell> _cellMap = new();

	public PatternEditor()
	{
		InitializeComponent();
		BuildGrid();
	}

	private void BuildGrid()
	{
		const int rows = 5;
		const int cols = 5;

		PatternGrid.RowDefinitions.Clear();
		PatternGrid.ColumnDefinitions.Clear();
		PatternGrid.Children.Clear();
		_borderMap.Clear();

		for (int r = 0; r < rows; r++)
			PatternGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

		for (int c = 0; c < cols; c++)
			PatternGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

		for (int r = 0; r < rows; r++)
		{
			for (int c = 0; c < cols; c++)
			{
				int row = r; // capture loop variable
				int col = c;

				Border border = new()
				{
					// ... your existing styling
				};

				var tap = new TapGestureRecognizer();
				tap.Tapped += (_, _) => ToggleCell(row, col); // use captured vars
				border.GestureRecognizers.Add(tap);

				PatternGrid.Children.Add(border);
				Grid.SetRow(border, r);
				Grid.SetColumn(border, c);
				_borderMap[(r, c)] = border;

				AddLetter(c, r);
			}
		}

		CreateStarInCenter();
		UpdatePatternVisuals();
	}

	private void AddLetter(int col, int row)
	{
		string letter = "BINGO"[col].ToString();
		Label label = new()
		{
			Text = letter,
			TextColor = Color.FromArgb("#303030"),
			FontSize = 16,
			Opacity = (row == 2 && col == 2) ? 0.1 : 0.35,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			InputTransparent = true // allows tap to pass to Border below
		};

		Grid.SetRow(label, row);
		Grid.SetColumn(label, col);
		PatternGrid.Children.Add(label);
	}

	private void CreateStarInCenter()
	{
		GraphicsView starView = new()
		{
			Drawable = new StarDrawable(),
			InputTransparent = true
		};

		Grid.SetRow(starView, 2);
		Grid.SetColumn(starView, 2);
		PatternGrid.Children.Add(starView);
	}

	private void SetPatternCells(IEnumerable<PatternCell> cells)
	{
		_cellMap.Clear();
		foreach (var cell in cells)
			_cellMap[(cell.Row, cell.Col)] = cell;

		UpdatePatternVisuals();
	}

	private void ToggleCell(int row, int col)
	{
		//System.Diagnostics.Debug.WriteLine($"ToggleCell called for ({row},{col})");

		if (_cellMap.TryGetValue((row, col), out var cell))
		{
			//System.Diagnostics.Debug.WriteLine($"Toggled ({row},{col}) -> {cell.IsActive}");

			cell.IsActive = !cell.IsActive;
			UpdatePatternVisuals();
		}
	}

	private void UpdatePatternVisuals()
	{
		foreach (var pos in _borderMap.Keys)
		{
			bool isActive = _cellMap.TryGetValue(pos, out var cell) && cell.IsActive;
			_borderMap[pos].Background = isActive ? Colors.Goldenrod : Colors.LightGray;
		}
	}
}
