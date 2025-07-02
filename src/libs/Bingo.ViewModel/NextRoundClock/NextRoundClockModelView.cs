using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bingo.ViewModel.NextRoundClock;

using Timer = System.Timers.Timer;

public class NextRoundClockViewModel : INotifyPropertyChanged
{
	private enum TimerMode { Hidden, Countdown, Elapsed }

	private DateTime? _targetTime;
	private TimerMode _mode = TimerMode.Hidden;
	private readonly Timer _timer;

	public bool IsVisible => _mode != TimerMode.Hidden;
	public bool IsElapsed => _mode == TimerMode.Elapsed;
	public bool IsReadOnly { get; set; } = false;

	public string? DisplayScheduled => _targetTime?.ToLocalTime().ToString("h:mm tt");

	public string? DisplayCountdown =>
		_mode switch
		{
			TimerMode.Countdown => FormatTimeSpan(Remaining),
			TimerMode.Elapsed => "Oops. Late… +" + FormatTimeSpan(DateTime.UtcNow - _targetTime!.Value),
			_ => null
		};

	public string CountdownColor =>
		_mode == TimerMode.Countdown && Remaining <= TimeSpan.FromSeconds(10)
			? "Crimson"
			: "#FFAA44";

	private TimeSpan Remaining => _targetTime is null ? TimeSpan.Zero : _targetTime.Value - DateTime.UtcNow;

	public NextRoundClockViewModel()
	{
		_timer = new Timer(1000);
		_timer.Elapsed += (_, _) => OnTick();
	}

	public void SetTimer(TimeSpan delay)
	{
		_targetTime = DateTime.UtcNow + delay;
		_mode = TimerMode.Countdown;
		_timer.Start();

		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(IsElapsed));
		OnPropertyChanged(nameof(DisplayScheduled));
		OnPropertyChanged(nameof(DisplayCountdown));
		OnPropertyChanged(nameof(CountdownColor));
	}

	public void CancelTimer()
	{
		_timer.Stop();
		_mode = TimerMode.Hidden;
		_targetTime = null;

		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(IsElapsed));
		OnPropertyChanged(nameof(DisplayScheduled));
		OnPropertyChanged(nameof(DisplayCountdown));
		OnPropertyChanged(nameof(CountdownColor));
	}

	private void OnTick()
	{
		if (_targetTime is null)
			return;

		if (_mode == TimerMode.Countdown && DateTime.UtcNow >= _targetTime)
		{
			_mode = TimerMode.Elapsed;
			OnPropertyChanged(nameof(IsElapsed));
			OnPropertyChanged(nameof(DisplayScheduled));
		}

		OnPropertyChanged(nameof(DisplayCountdown));
		OnPropertyChanged(nameof(CountdownColor));
	}

	private string FormatTimeSpan(TimeSpan span)
	{
		span = span < TimeSpan.Zero ? TimeSpan.Zero : span;
		return $"{(int)span.TotalMinutes:D2}:{span.Seconds:D2}";
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string? name = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
