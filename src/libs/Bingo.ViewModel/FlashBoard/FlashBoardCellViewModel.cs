using Bingo.Core.FlashBoard;
using Bingo.Core.FlashBoard.Events;
using System.ComponentModel;
using System.Diagnostics;

namespace Bingo.ViewModel.FlashBoard
{
	public class FlashBoardCellViewModel : INotifyPropertyChanged
	{
		private FlashBoardNumber _model;

		public FlashBoardNumber Model
		{
			get => _model;
			set
			{
				if (_model != value)
				{
					if (_model is not null)
						_model.IsCalledChanged -= OnIsCalledChanged;

					_model = value;

					if (_model is not null)
						_model.IsCalledChanged += OnIsCalledChanged;

					OnPropertyChanged(nameof(IsCalled));
					OnPropertyChanged(nameof(CanToggle));
				}
			}
		}

		public FlashBoardCellViewModel(FlashBoardNumber model)
		{
			_model = model;
			_model.IsCalledChanged += (_, _) =>
			{
				OnPropertyChanged(nameof(IsCalled));
				OnPropertyChanged(nameof(CanToggle));
			};
		}

		public FlashBoardCellViewModel(int number, FlashBoardGroup parent)
			: this(new FlashBoardNumber(number, parent))
		{
		}

		public FlashBoardGroup Parent => _model.Parent;
		public int Number => _model.Number;
		public char Letter => _model.Parent.Letter;
				
		public bool IsCalled => _model.IsCalled;

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

		private FlashBoardViewModel? _parentBoard;

		public void SetParentBoard(FlashBoardViewModel boardVM)
		{
			if (_model.Number == 67)
				Debug.WriteLine($"[FlashBoardCellViewModel.SetParentBoard] Cell {_model.Number} now listening to board {boardVM}");

			_parentBoard = boardVM;

			boardVM.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(boardVM.IsInteractive))
					OnPropertyChanged(nameof(CanToggle));
			};

			//Initial state update
			OnPropertyChanged(nameof(CanToggle));
		}

		public void TryToggle()
		{
			if (!CanToggle)
				return;

			_parentBoard?.ToggleCalled(_model.Number);
		}

		private void OnIsCalledChanged(object? sender, FlashBoardCalledChangedEventArgs e)
		{
			if ( this.Number == 67 )
				Debug.WriteLine($"[FlashBoardCellViewModel.OnIsCalledChanged] Cell {_model.Number} IsCalled changed to {_model.IsCalled} by {e.SourceTag}");			

			OnPropertyChanged(nameof(IsCalled));
			OnPropertyChanged(nameof(CanToggle));
			_parentBoard?.RaiseAnimationRequest(_model.Number, e.SourceTag);
		}

		public bool CanToggle
		{
			get
			{
				bool result = _parentBoard?.IsInteractive == true && !_model.IsCalled;
				if ( _model.Number == 67 )
					Debug.WriteLine($"[FlashBoardCellViewModel.CanToggle] Cell {_model.Number}: IsCalled={_model.IsCalled} IsInteractive={_parentBoard?.IsInteractive} → CanToggle={result}");
				return result;
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
