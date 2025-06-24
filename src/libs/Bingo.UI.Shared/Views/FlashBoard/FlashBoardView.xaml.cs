using Bingo.ViewModel.FlashBoard;

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

        public static readonly BindableProperty IsInteractiveProperty =
            BindableProperty.Create(nameof(IsInteractive), typeof(bool), typeof(FlashBoardView), false);

        public bool IsInteractive
        {
            get => (bool)GetValue(IsInteractiveProperty);
            set => SetValue(IsInteractiveProperty, value);
        }

        private void BuildGrid()
        {
            CellGrid.Children.Clear();
            CellGrid.RowDefinitions.Clear();
            CellGrid.ColumnDefinitions.Clear();

            // 5 rows for B-I-N-G-O
            for (int i = 0; i < 5; i++)
                CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            // 6 columns (1 header + 5 number cells per row)
            for (int i = 0; i < 6; i++)
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            if (ViewModel?.Groups == null)
                return;

            for (int row = 0; row < ViewModel.Groups.Count; row++)
            {
                // Add B-I-N-G-O header
                Label letter = new()
                {
                    Text = ViewModel.Groups[row].Letter.ToString(),
                    TextColor = Colors.White,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                Grid.SetRow(letter, row);
                Grid.SetColumn(letter, 0);
                CellGrid.Children.Add(letter);

                // Add number cells
                for (int col = 0; col < ViewModel.Groups[row].Cells.Count; col++)
                {
                    FlashBoardCellView cellView = new()
                    {
                        BindingContext = ViewModel.Groups[row].Cells[col]
                    };
                    Grid.SetRow(cellView, row);
                    Grid.SetColumn(cellView, col + 1); // +1 to offset letter
                    CellGrid.Children.Add(cellView);
                }
            }
        }
    }
}
