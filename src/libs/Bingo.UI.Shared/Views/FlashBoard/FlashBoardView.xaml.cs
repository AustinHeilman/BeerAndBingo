using Bingo.UI.Shared.Device;
using Bingo.UI.Shared.Services;
using Bingo.ViewModel.FlashBoard;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace Bingo.UI.Shared.Views.FlashBoard
{
    public partial class FlashBoardView : ContentView
    {
        private readonly StyleBindingService _styleService = new(new FontStyleService(new MauiDeviceInfoProvider()));

        public FlashBoardView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsInteractiveProperty =
            BindableProperty.Create(nameof(IsInteractive), typeof(bool), typeof(FlashBoardView), false);

        public bool IsInteractive
        {
            get => (bool)GetValue(IsInteractiveProperty);
            set => SetValue(IsInteractiveProperty, value);
        }

        public FlashBoardViewModel ViewModel
        {
            get => (FlashBoardViewModel)BindingContext;
            set
            {
                BindingContext = value;
                BuildGrid();
            }
        }

        private void BuildGrid()
        {
            CellGrid.Children.Clear();
            CellGrid.RowDefinitions.Clear();
            CellGrid.ColumnDefinitions.Clear();

            // 5 rows: B, I, N, G, O
            for (int i = 0; i < 5; i++)
                CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            // 16 columns: 1 for label + 15 numbers
            for (int i = 0; i < 16; i++)
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var fontSet = _styleService.GetFontSet();
            var groups = ViewModel?.Groups;
            if (groups is null)
                return;

            for (int row = 0; row < groups.Count; row++)
            {
                var group = groups[row];

                // Left-side letter label (column 0)
                var headerLabelStyle = Application.Current?.Resources["FlashBoardHeaderLabel"] as Style;
                if (headerLabelStyle != null)
                {
                    var label = new Label
                    {
                        Text = group.Letter.ToString(),
                        Style = headerLabelStyle
                    };
                    Grid.SetRow(label, row);
                    Grid.SetColumn(label, 0);
                    CellGrid.Children.Add(label);
                }

                // Number cells (columns 1–15)
                for (int col = 0; col < group.Cells.Count; col++)
                {
                    var cellView = new FlashBoardCellView
                    {
                        BindingContext = group.Cells[col]
                    };

                    if (IsInteractive)
                    {
                        var tap = new TapGestureRecognizer
                        {
                            Command = ViewModel?.ToggleCallCommand, // Safely access ViewModel
                            CommandParameter = group.Cells[col].Number
                        };
                        cellView.GestureRecognizers.Add(tap);
                    }

                    Grid.SetRow(cellView, row);
                    Grid.SetColumn(cellView, col + 1);
                    CellGrid.Children.Add(cellView);
                }
            }
        }
    }
}
