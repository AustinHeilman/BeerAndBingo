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

		public new event PropertyChangedEventHandler? PropertyChanged;

		protected new void OnPropertyChanged(string propertyName) =>
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
			// Clear and configure grid structure
			CellGrid.Children.Clear();
			CellGrid.RowDefinitions.Clear();
			CellGrid.ColumnDefinitions.Clear();

			for (int i = 0; i < 5; i++)
				CellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // B, I, N, G, O

			for (int i = 0; i < 16; i++)
				CellGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 1 label + 15 numbers

			// Get font and style resources
			FontSet fontSet = _styleService.GetFontSet();
			System.Collections.ObjectModel.ObservableCollection<FlashBoardGroupViewModel>? groups = FlashBoardVM?.Groups;
			if (groups is null)
				return;

			// Populate each row (group)
			for (int row = 0; row < groups.Count; row++)
			{
				FlashBoardGroupViewModel group = groups[row];

				// Add left-side header label
				if (Application.Current?.Resources["FlashBoardHeaderLabel"] is Style headerLabelStyle)
				{
					Label label = new()
					{
						Text = group.Letter.ToString(),
						Style = headerLabelStyle
					};

					Grid.SetRow(label, row);
					Grid.SetColumn(label, 0);
					CellGrid.Children.Add(label);
				}

				// Add cells
				for (int col = 0; col < group.Cells.Count; col++)
				{
					FlashBoardCellViewModel cellVM = group.Cells[col];
					FlashBoardCellView cellView = new()
					{
						BindingContext = cellVM
					};
					cellView.FlashDebugColor();

					// Bind interactivity visuals
					cellView.SetBinding(
						FlashBoardCellView.CanToggleProperty,
						new Binding(nameof(cellVM.CanToggle))
					);

					// Hook up gesture control
					cellView.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = FlashBoardVM?.ToggleCallCommand ?? throw new NullReferenceException(nameof(FlashBoardVM)),
						CommandParameter = cellVM.Number
					});

					Grid.SetRow(cellView, row);
					Grid.SetColumn(cellView, col + 1);
					CellGrid.Children.Add(cellView);
				}
			}
		}

	}
}
