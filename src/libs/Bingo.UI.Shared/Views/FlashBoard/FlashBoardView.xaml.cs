using Bingo.Core.Device.Fonts;
using Bingo.UI.Shared.Services;
using Bingo.ViewModel.FlashBoard;
using System.ComponentModel;

namespace Bingo.UI.Shared.Views.FlashBoard
{
	public partial class FlashBoardView : ContentView, INotifyPropertyChanged
	{
		private readonly StyleBindingService _styleService = new(new MauiDeviceInfoProvider());

		public FlashBoardView()
		{
			InitializeComponent();
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public bool IsInteractive => FlashBoardVM?.IsInteractive ?? false;

		public FlashBoardViewModel FlashBoardVM
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

			// Define 5 rows: B, I, N, G, O
			for (int i = 0; i < 5; i++)
				CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

			// Define 16 columns: 1 for label, 15 for numbers
			for (int i = 0; i < 16; i++)
				CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			FontSet fontSet = _styleService.GetFontSet();
			var groups = FlashBoardVM?.Groups;
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

					var tap = new TapGestureRecognizer
					{
						Command = FlashBoardVM.ToggleCallCommand,
						CommandParameter = group.Cells[col].Number
					};
					cellView.GestureRecognizers.Add(tap);

					Grid.SetRow(cellView, row);
					Grid.SetColumn(cellView, col + 1);
					CellGrid.Children.Add(cellView);
				}
			}
		}
	}
}
