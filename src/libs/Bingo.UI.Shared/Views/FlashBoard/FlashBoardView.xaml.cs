using Bingo.ModelView.FlashBoard;
using Bingo.UI.Shared.Services;
using Bingo.UI.Shared.Styles;

namespace Bingo.UI.Shared.Views.FlashBoard;

public partial class FlashBoardView : ContentView
{
    private readonly StyleBindingService _styleBinding;

    public FlashBoardView(StyleBindingService styleBinding)
    {
        InitializeComponent();
        _styleBinding = styleBinding;
    }

    #region ViewModel Bindable Property

    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        nameof(ViewModel),
        typeof(FlashBoardViewModel),
        typeof(FlashBoardView),
        propertyChanged: OnViewModelChanged);

    public FlashBoardViewModel ViewModel
    {
        get => (FlashBoardViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FlashBoardView view && newValue is FlashBoardViewModel vm)
        {
            view.BuildFlashBoard(vm);
        }
    }

    public static readonly BindableProperty IsInteractiveProperty = BindableProperty.Create(
    nameof(IsInteractive),
    typeof(bool),
    typeof(FlashBoardView),
    defaultValue: false);

    #endregion

    public bool IsInteractive
    {
        get => (bool)GetValue(IsInteractiveProperty);
        set => SetValue(IsInteractiveProperty, value);
    }

    private void BuildFlashBoard(FlashBoardViewModel vm)
    {
        var font = _styleBinding.GetFontSet();

        FlashBoardGrid.RowDefinitions.Clear();
        FlashBoardGrid.ColumnDefinitions.Clear();
        FlashBoardGrid.Children.Clear();

        for (int i = 0; i < 6; i++) FlashBoardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        for (int i = 0; i < 5; i++) FlashBoardGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (int col = 0; col < 5; col++)
        {
            var header = CreateHeaderCell((char)('B' + col));
            FlashBoardGrid.Add(header, col, 0);
        }

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                int number = (col * 15) + row + 1;
                var cell = CreateNumberCell(number);
                FlashBoardGrid.Add(cell, col, row + 1);
            }
        }
    }

    private Border CreateHeaderCell(char letter)
    {
        var font = _styleBinding.GetFontSet();
        var label = _styleBinding.CreateStyledLabel(font.Header, letter.ToString());

        label.TextColor = ThemeHelpers.GetAppColor("FlashHeaderTextColor", Colors.LightYellow);

        return new Border
        {
            BackgroundColor = Colors.Transparent,
            Content = label,
            Stroke = Colors.Black,
            StrokeThickness = 1,
            Margin = new Thickness(1)
        };
    }

    private Border CreateNumberCell(int number)
    {
        var font = _styleBinding.GetFontSet();
        var label = _styleBinding.CreateStyledLabel(font.Number, number.ToString());

        label.TextColor = ThemeHelpers.GetAppColor("FlashCellTextColor", Colors.Black);

        bool isCalled = false; // Placeholder for future GameService integration
        string bgKey = isCalled ? "FlashCellBGColor_Called" : "FlashCellBGColor_Uncalled";

        return new Border
        {
            BackgroundColor = ThemeHelpers.GetAppColor(bgKey, Colors.DarkGray),
            Content = label,
            Stroke = Colors.Black,
            StrokeThickness = 1,
            Margin = new Thickness(1)
        };
    }
}
