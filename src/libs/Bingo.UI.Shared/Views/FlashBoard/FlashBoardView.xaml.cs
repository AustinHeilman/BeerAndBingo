using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Bingo.ViewModel.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.UI.Shared.Views.FlashBoard
{
    public partial class FlashBoardView : ContentView
    {
        public bool IsInteractive { get; set; } = false;

        public FlashBoardView()
        {
            InitializeComponent();
            BuildGrid();

            if (BindingContext is FlashBoardViewModel vm)
            {
                vm.NumberCalledAnimationRequested += OnNumberCalledAnimationRequested;
            }
        }

        private void BuildGrid()
        {
            const int columns = 5;
            const int rows = 15;

            CellGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < columns; i++)
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            CellGrid.RowDefinitions.Clear();
            for (int i = 0; i < rows; i++)
                CellGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            if (BindingContext is not FlashBoardViewModel vm)
                return;

            foreach (var cell in vm.Cells)
            {
                var label = new Label
                {
                    Text = cell.Number.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    TextColor = Colors.Black
                };

                var border = new Border
                {
                    Stroke = Colors.Black,
                    StrokeThickness = 1,
                    BackgroundColor = Colors.White,
                    WidthRequest = 48,
                    HeightRequest = 48,
                    Padding = 4,
                    Content = label,
                    AutomationId = cell.Number.ToString()
                };

                int col = GetColumnIndex(cell);
                int row = GetRowIndex(cell, vm);

                CellGrid.Children.Add(border);
                Grid.SetColumn(border, col);
                Grid.SetRow(border, row);
            }
        }

        private int GetColumnIndex(FlashBoardCellViewModel vm) => vm.Letter switch
        {
            'B' => 0,
            'I' => 1,
            'N' => 2,
            'G' => 3,
            'O' => 4,
            _ => 0
        };

        private int GetRowIndex(FlashBoardCellViewModel vm, FlashBoardViewModel viewModel)
        {
            return viewModel.Cells
                .Where(c => c.Letter == vm.Letter)
                .OrderBy(c => c.Number)
                .ToList()
                .FindIndex(c => c.Number == vm.Number);
        }

        private void OnNumberCalledAnimationRequested(int number, FlashBoardEventSource source)
        {
            var border = CellGrid.Children
                .OfType<Border>()
                .FirstOrDefault(b => b.AutomationId == number.ToString());

            if (border is not null)
            {
                AnimateCell(border, source);
            }
        }

        private async void AnimateCell(Border border, FlashBoardEventSource source)
        {
            var flashColor = source switch
            {
                FlashBoardEventSource.Manual => Colors.Green,
                FlashBoardEventSource.Random => Colors.Orange,
                FlashBoardEventSource.Replay => Colors.Cyan,
                FlashBoardEventSource.Undo => Colors.Gray,
                FlashBoardEventSource.Redo => Colors.Blue,
                _ => Colors.Yellow
            };

            var originalColor = border.BackgroundColor;
            border.BackgroundColor = flashColor;

            await border.ScaleTo(1.2, 100, Easing.CubicOut);
            await border.ScaleTo(1.0, 100, Easing.CubicIn);
            await Task.Delay(150);

            border.BackgroundColor = originalColor;
        }

        public FlashBoardViewModel ViewModel
        {
            get => (FlashBoardViewModel)BindingContext;
            set => BindingContext = value;
        }
    }
}
