using Bingo.ViewModel.Patterns;

namespace Bingo.UI.Shared.Views.Patterns.PatternDisplay;

public partial class PatternDisplayView : ContentView
{
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

    public PatternDisplayView()
    {
        InitializeComponent();
        BuildGrid();
    }

    private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PatternDisplayView view)
        {
            view.BindingContext = newValue;
        }
    }

    private void BuildGrid()
    {
        const int rows = 5;
        const int cols = 15;

        PatternGrid.RowDefinitions.Clear();
        PatternGrid.ColumnDefinitions.Clear();
        PatternGrid.Children.Clear();

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
                    Color = Colors.Transparent,
                    BindingContext = (r, c)
                };
                PatternGrid.Children.Add(box);
                Grid.SetRow(box, r);
                Grid.SetColumn(box, c);
            }
        }

        UpdatePatternVisuals();
    }

    public static readonly BindableProperty PatternCellsProperty =
        BindableProperty.Create(nameof(PatternCells), typeof(ISet<(int, int)>), typeof(PatternDisplayView), new HashSet<(int, int)>(), propertyChanged: (_, __, ___) => { });

    public ISet<(int Row, int Col)> PatternCells
    {
        get => (ISet<(int, int)>)GetValue(PatternCellsProperty);
        set
        {
            SetValue(PatternCellsProperty, value);
            UpdatePatternVisuals();
        }
    }

    private void UpdatePatternVisuals()
    {
        foreach (var child in PatternGrid.Children)
        {
            if (child is BoxView box && box.BindingContext is ValueTuple<int, int> pos)
            {
                box.Color = PatternCells?.Contains(pos) == true ? Colors.MediumPurple : Colors.Transparent;
            }
        }
    }
}
