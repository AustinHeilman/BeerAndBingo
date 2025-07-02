using Bingo.ViewModel.NextRoundClock;

namespace Bingo.UI.Shared.Views.NextRoundClock;

public partial class NextRoundClock : ContentView
{
	public NextRoundClockViewModel ViewModel { get; } = new();

	public event EventHandler? RequestClose;

	public NextRoundClock()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnSetTimerClicked(object sender, EventArgs e)
	{
		if (double.TryParse(MinutesEntry.Text, out double minutes) && minutes > 0)
			ViewModel.SetTimer(TimeSpan.FromMinutes(minutes));
	}

	private void OnCancelClicked(object sender, EventArgs e)
	{
		RequestClose?.Invoke(this, EventArgs.Empty); // signal host to handle full shutdown
	}
}
