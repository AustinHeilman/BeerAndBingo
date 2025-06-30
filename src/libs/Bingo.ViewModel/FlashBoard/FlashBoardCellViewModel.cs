using Bingo.Core.Games.Bingo.FlashBoard;
using Bingo.Core.Games.Bingo.FlashBoard.Events;
using System.ComponentModel;

namespace Bingo.ViewModel.FlashBoard
{
	public class FlashBoardCellViewModel : INotifyPropertyChanged
	{
		private readonly FlashBoardNumber _model;

		public FlashBoardCellViewModel(FlashBoardNumber model)
		{
			_model = model;
			_model.IsCalledChanged += (s, e) =>
			{
				IsCalled = e.NewValue;
				SourceTag = e.SourceTag;
			};
		}

		public FlashBoardCellViewModel(int number, FlashBoardGroup parent)
			: this(new FlashBoardNumber(number, parent))
		{
		}

		public FlashBoardGroup Parent => _model.Parent;
		public int Number => _model.Number;
		public char Letter => _model.Parent.Letter;

		private bool _isCalled;
		public bool IsCalled
		{
			get => _isCalled;
			private set
			{
				if (_isCalled != value)
				{
					_isCalled = value;
					OnPropertyChanged(nameof(IsCalled));
				}
			}
		}

		private FlashBoardEventSource _sourceTag = FlashBoardEventSource.Unknown;
		public FlashBoardEventSource SourceTag
		{
			get => _sourceTag;
			private set
			{
				if (_sourceTag != value)
				{
					_sourceTag = value;
					OnPropertyChanged(nameof(SourceTag));
				}
			}
		}

		private bool _groupCompleted;
		public bool GroupCompleted
		{
			get => _groupCompleted;
			set
			{
				if (_groupCompleted != value)
				{
					_groupCompleted = value;
					OnPropertyChanged(nameof(GroupCompleted));
				}
			}
		}

		public FlashBoardNumber Model => _model;

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
