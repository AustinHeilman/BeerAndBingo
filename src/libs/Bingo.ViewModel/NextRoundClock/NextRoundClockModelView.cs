using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Bingo.ViewModel.NextRoundClock;

public class NextRoundClockViewModel : INotifyPropertyChanged
{
	private DateTime? _nextRoundTime;
	private readonly System.Timers.Timer _timer;

	public bool IsVisible => _nextRoundTime.HasValue;
	public bool IsReadOnly { get; set; } = false;

	public string? DisplayScheduled => _nextRoundTime?.ToLocalTime().ToString("h:mm tt");
	public string? DisplayCountdown => _nextRoundTime is null
		? null
		: FormatCountdown(_nextRoundTime.Value - DateTime.UtcNow);

	public NextRoundClockViewModel()
	{
		_timer = new System.Timers.Timer(1000); // Update every second
		_timer.Elapsed += (_, _) => OnTick();
	}

	public void SetTimer(TimeSpan delay)
	{
		_nextRoundTime = DateTime.UtcNow + delay;
		_timer.Start();
		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(DisplayScheduled));
		OnPropertyChanged(nameof(DisplayCountdown));
	}

	public void CancelTimer()
	{
		_timer.Stop();
		_nextRoundTime = null;
		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(DisplayScheduled));
		OnPropertyChanged(nameof(DisplayCountdown));
	}

	private void OnTick()
	{
		if (_nextRoundTime is null)
			return;

		var remaining = _nextRoundTime.Value - DateTime.UtcNow;

		if (remaining <= TimeSpan.Zero)
		{
			CancelTimer();
			return;
		}

		OnPropertyChanged(nameof(DisplayCountdown));
	}

	private string FormatCountdown(TimeSpan span) =>
		$"{(int)span.TotalMinutes:D2}:{span.Seconds:D2}";

	public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string? name = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
