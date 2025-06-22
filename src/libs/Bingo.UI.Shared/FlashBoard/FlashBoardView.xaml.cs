using Bingo.ModelView.FlashBoard;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Bingo.UI.Shared.FlashBoard;

public partial class FlashBoardView : ContentView
{
    public HashSet<int> CalledNumbers { get; set; } = new();

    public bool IsCaller { get; set; } = false;

    public Action<int>? ToggleCallRequested { get; set; }

    public FlashBoardView()
    {
        InitializeComponent();
        BuildFlashBoard();
    }

    private void BuildFlashBoard()
    {
        int number = 1;

        for (int col = 0; col < 15; col++)
        {
            for (int row = 1; row <= 5; row++)
            {
                var button = CreateCell(number);
                BoardGrid.Add(button, col, row);
                number++;
            }
        }
    }

    private View CreateCell(int number)
    {
        var label = new Label
        {
            Text = number.ToString(),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            Padding = new Thickness(8),
            Margin = new Thickness(2)
        };

        var border = new Border
        {
            Stroke = Colors.Transparent,
            StrokeThickness = 2,
            Content = label
        };

        if (IsCaller)
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, __) => ToggleCallRequested?.Invoke(number);
            border.GestureRecognizers.Add(tap);
        }

        return border;
    }

    public void RefreshCalledNumbers(HashSet<int> called)
    {
        CalledNumbers = called;
        // TODO: Add logic to visually update cells
    }

    public static readonly BindableProperty ViewModelProperty =
    BindableProperty.Create(nameof(ViewModel), typeof(FlashBoardViewModel), typeof(FlashBoardView), propertyChanged: OnViewModelChanged);

    public FlashBoardViewModel ViewModel
    {
        get => (FlashBoardViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FlashBoardView view && newValue is FlashBoardViewModel vm)
        {
            view.RefreshBoard(vm);
        }
    }

    private readonly Dictionary<int, Border> _cellMap = new();
    private void RefreshBoard(FlashBoardViewModel vm)
    {
        BoardGrid.Children.Clear();
        _cellMap.Clear();

        int number = 1;
        for (int col = 0; col < 15; col++)
        {
            for (int row = 1; row <= 5; row++)
            {
                var border = CreateCell(number, vm);
                BoardGrid.Add(border, col, row);
                _cellMap[number] = border;
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
            Padding = 6
        };

        var border = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.LightGray,
            Content = label
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, __) => vm.ToggleCallCommand.Execute(number);
        border.GestureRecognizers.Add(tap);

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
