using Bingo.ModelView.FlashBoard;

namespace Bingo.UI.Shared.Views.FlashBoard;

public partial class FlashBoardView : ContentView
{
    public static readonly BindableProperty ViewModelProperty =
        BindableProperty.Create(nameof(ViewModel), typeof(FlashBoardViewModel), typeof(FlashBoardView), propertyChanged: OnViewModelChanged);

    public static readonly BindableProperty IsInteractiveProperty =
        BindableProperty.Create(nameof(IsInteractive), typeof(bool), typeof(FlashBoardView), true);

    public FlashBoardViewModel ViewModel
    {
        get => (FlashBoardViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public bool IsInteractive
    {
        get => (bool)GetValue(IsInteractiveProperty);
        set => SetValue(IsInteractiveProperty, value);
    }

    private readonly Dictionary<int, Border> _cellMap = new();

    public FlashBoardView()
    {
        InitializeComponent();
    }

    private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FlashBoardView view && newValue is FlashBoardViewModel vm)
        {
            view.BuildFlashBoard(vm);
        }
    }

    private void BuildFlashBoard(FlashBoardViewModel vm)
    {
        BoardGrid.Children.Clear();
        _cellMap.Clear();

        int number = 1;
        for (int col = 0; col < 15; col++)
        {
            for (int row = 1; row <= 5; row++)
            {
                var cell = CreateCell(number, vm);
                BoardGrid.Add(cell, col, row);
                _cellMap[number] = cell;
                number++;
            }
        }

        UpdateCalledVisuals(vm);
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(vm.CalledNumbers))
                UpdateCalledVisuals(vm);
        };
    }

    private Border CreateCell(int number, FlashBoardViewModel vm)
    {
        var label = new Label
        {
            Text = number.ToString(),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black,
            Padding = new Thickness(6)
        };

        var border = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.LightGray,
            Content = label
        };

        if (IsInteractive)
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, __) => vm.ToggleCallCommand.Execute(number);
            border.GestureRecognizers.Add(tap);
        }

        return border;
    }

    private void UpdateCalledVisuals(FlashBoardViewModel vm)
    {
        foreach (var kvp in _cellMap)
        {
            var number = kvp.Key;
            var border = kvp.Value;
            bool isCalled = vm.CalledNumbers.Contains(number);
            border.BackgroundColor = isCalled ? Colors.Gold : Colors.LightGray;
            border.Stroke = isCalled ? Colors.Yellow : Colors.Transparent;
        }
    }
}
