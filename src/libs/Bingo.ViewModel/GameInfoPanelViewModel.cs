using Bingo.Core.Domain;
using Bingo.Core.Domain.Bingo;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bingo.ViewModel.GameInfo;

public class GameInfoPanelViewModel : INotifyPropertyChanged
{
	private readonly GameSessionState<int> _session;

	public string CurrentCallDisplay => Format(_session.CurrentItem);
	public string PreviousCallDisplay => Format(_session.PreviousItem);

	public string CurrentCallLabeled => $"Current: {CurrentCallDisplay}";
	public string PreviousCallLabeled => $"Previous: {PreviousCallDisplay}";

	public string GameRoundsText => $"Game Rounds: {_session.Round}";

	public GameInfoPanelViewModel(GameSessionState<int> session)
	{
		_session = session;

		_session.ItemCalled += (_, _) => OnGameStateChanged();
		_session.UndoPerformed += (_, _) => OnGameStateChanged();
		_session.RedoPerformed += (_, _) => OnGameStateChanged();
		_session.NewGameStarted += (_, _) => OnGameStateChanged();
	}

	private string Format(int? number)
	{
		if (number is null || number is < BingoSession.Min or > BingoSession.Max)
			return "None";

		return BingoSession.FormatCall(number.Value);
	}


	private void OnGameStateChanged()
	{
		OnPropertyChanged(nameof(CurrentCallDisplay));
		OnPropertyChanged(nameof(PreviousCallDisplay));
		OnPropertyChanged(nameof(CurrentCallLabeled));
		OnPropertyChanged(nameof(PreviousCallLabeled));
		OnPropertyChanged(nameof(GameRoundsText));
	}


	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
