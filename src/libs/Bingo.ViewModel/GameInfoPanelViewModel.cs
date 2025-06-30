using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bingo.ViewModels
{
	public class GameInfoPanelViewModel : INotifyPropertyChanged
	{
		public GameInfoPanelViewModel()
		{
			// Initialize properties with default values
			CurrentCallDisplay = "Current Call: None";
			PreviousCallDisplay = "Previous Call: None";
			GameRoundsText = "Game Rounds: 0";
		}

		private string _currentCallDisplay;
		private string _previousCallDisplay;
		private string _gameRoundsText;

		public string CurrentCallDisplay
		{
			get => _currentCallDisplay;
			set
			{
				if (_currentCallDisplay != value)
				{
					_currentCallDisplay = value;
					OnPropertyChanged();
				}
			}
		}

		public string PreviousCallDisplay
		{
			get => _previousCallDisplay;
			set
			{
				if (_previousCallDisplay != value)
				{
					_previousCallDisplay = value;
					OnPropertyChanged();
				}
			}
		}

		public string GameRoundsText
		{
			get => _gameRoundsText;
			set
			{
				if (_gameRoundsText != value)
				{
					_gameRoundsText = value;
					OnPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}