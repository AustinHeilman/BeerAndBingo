using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using Microsoft.Maui.Controls.Shapes;

namespace Bingo.UI.Shared.Views.Patterns.PatternDisplay;

public partial class PatternDisplayView : ContentView
{
	public static readonly BindableProperty PatternCellsProperty =
		BindableProperty.Create(
			nameof(PatternCells),
			typeof(IEnumerable<PatternCell>),
			typeof(PatternDisplayView),
			defaultValue: Enumerable.Empty<PatternCell>(),
			propertyChanged: (bindable, oldVal, newVal) =>
			{
				if (bindable is PatternDisplayView view && newVal is IEnumerable<PatternCell> cells)
				{
					view.SetPatternCells(cells);
				}
			});

	public IEnumerable<PatternCell> PatternCells
	{
		get => (IEnumerable<PatternCell>)GetValue(PatternCellsProperty);
		set => SetValue(PatternCellsProperty, value);
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

	private readonly Dictionary<(int, int), Border> _borderMap = new();
	private readonly Dictionary<(int, int), PatternCell> _cellMap = new();

	public PatternDisplayView()
	{
		InitializeComponent();
		BuildGrid();
	}

	private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not PatternDisplayView view || newValue is not PatternDisplayViewModel vm)
			return;

		view.BindingContext = vm;
		view.SetPatternCells(vm.PatternCells);
		vm.PatternCells.CollectionChanged += (_, __) => view.SetPatternCells(vm.PatternCells);
	}

	private void SetPatternCells(IEnumerable<PatternCell> cells)
	{
		_cellMap.Clear();
		foreach (PatternCell cell in cells)
			_cellMap[(cell.Row, cell.Col)] = cell;

		UpdatePatternVisuals();
	}

	private void BuildGrid()
	{
		int rows = PatternGridSettings.PatternRowCount;
		int cols = PatternGridSettings.PatternColCount;

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
				Border border = new()
				{
					Padding = 0,
					Margin = new Thickness(0),
					Stroke = Colors.Black,
					StrokeThickness = 1,
					Background = Colors.LightGray,
					StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(1) },
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
					BindingContext = (r, c)
				};

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
		string columnLetter = "BINGO"[col].ToString();

		Label label = new()
		{
			Text = columnLetter,
			TextColor = Color.FromArgb("#303030"),
			FontSize = 16,
			Opacity = (row == 2 && col == 2) ? 0.1 : 0.35,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
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
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill,
			Margin = new Thickness(4),
			InputTransparent = true
		};

		Grid.SetRow(starView, 2);
		Grid.SetColumn(starView, 2);
		PatternGrid.Children.Add(starView);
	}

	private void UpdatePatternVisuals()
	{
		foreach ((int, int) pos in _borderMap.Keys)
		{
			bool isActive = _cellMap.TryGetValue(pos, out PatternCell? cell) && cell.IsActive;
			_borderMap[pos].Background = isActive
				? Colors.Goldenrod
				: Colors.LightGray;
		}
	}
}
