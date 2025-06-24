using Bingo.ViewModel.FlashBoard;
using Microsoft.Maui.Controls;
using System.Linq;

namespace Bingo.UI.Shared.Views.FlashBoard
{
    public partial class FlashBoardView : ContentView
    {
        public FlashBoardView()
        {
            InitializeComponent();
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

        public bool IsInteractive { get; set; } = false;

        private void BuildGrid()
        {
            CellGrid.Children.Clear();
            CellGrid.RowDefinitions.Clear();
            CellGrid.ColumnDefinitions.Clear();

            // Define 15 rows
            for (int i = 0; i < 15; i++)
                CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            // Define 5 columns
            for (int i = 0; i < 5; i++)
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            if (ViewModel?.Cells == null)
                return;

            // Group by letter if available
            var groups = ViewModel.Cells
                .GroupBy(c => c.Letter)
                .OrderBy(g => "BINGO".IndexOf(g.Key));

            int col = 0;
            foreach (var group in groups)
            {
                var cells = group.OrderBy(c => c.Number).ToList();
                for (int row = 0; row < cells.Count && row < 15; row++)
                {
                    var cellView = new FlashBoardCellView
                    {
                        BindingContext = cells[row]
                    };

                    Grid.SetColumn(cellView, col);
                    Grid.SetRow(cellView, row);
                    CellGrid.Children.Add(cellView);
                }
                col++;
            }
        }
    }
}
