using Bingo.ViewModel.NextRoundClock;

namespace Bingo.UI.Shared.Views.NextRoundClock;

public partial class NextRoundClock : ContentView
{
	public NextRoundClockViewModel ViewModel { get; } = new();

	public NextRoundClock()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnSetTimerClicked(object sender, EventArgs e)
	{
		if (int.TryParse(MinutesEntry.Text, out int minutes) && minutes > 0)
			ViewModel.SetTimer(TimeSpan.FromMinutes(minutes));
	}

	private void OnCancelClicked(object sender, EventArgs e)
	{
		ViewModel.CancelTimer();
	}
}
