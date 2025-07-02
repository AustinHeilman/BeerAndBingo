using Bingo.Core.Device.Fonts;
using Bingo.UI.Shared.Services;
using Bingo.ViewModel.FlashBoard;

namespace Bingo.UI.Shared.Views.FlashBoard
{
	public partial class FlashBoardView : ContentView
	{
		private readonly StyleBindingService _styleService = new(new MauiDeviceInfoProvider());

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

		public BaseFlashBoardViewModel ViewModel
		{
			get => (BaseFlashBoardViewModel)BindingContext;
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

			// Define 5 rows: B, I, N, G, O
			for (int i = 0; i < 5; i++)
				CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

			// Define 16 columns: 1 for label, 15 for numbers
			for (int i = 0; i < 16; i++)
				CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			FontSet fontSet = _styleService.GetFontSet();
			var groups = ViewModel?.Groups;
			if (groups is null)
				return;

			for (int row = 0; row < groups.Count; row++)
			{
				var group = groups[row];

				// Left-side letter label
				if (Application.Current?.Resources["FlashBoardHeaderLabel"] is Style headerLabelStyle)
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

				// Number cells
				for (int col = 0; col < group.Cells.Count; col++)
				{
					var cellView = new FlashBoardCellView
					{
						BindingContext = group.Cells[col]
					};

					if (IsInteractive && ViewModel is InteractiveFlashBoardViewModel interactive)
					{
						var tap = new TapGestureRecognizer
						{
							Command = interactive.ToggleCallCommand,
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
