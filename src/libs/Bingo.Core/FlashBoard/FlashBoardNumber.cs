using Bingo.Core.FlashBoard.Events;
using System.Diagnostics;

namespace Bingo.Core.FlashBoard;

public class FlashBoardNumber
{
	public int Number { get; }
	public FlashBoardGroup Parent { get; }

	private bool _isCalled = false;

	public bool IsCalled
	{
		get => _isCalled;
		private set
		{
			if (_isCalled != value)
			{
				_isCalled = value;
				if (this.Number == 67)
					Debug.WriteLine($"[FlashBoardNumber.IsCalled] {Number}: set to {value}");
			}
		}
	}

	public FlashBoardEventSource SourceTag { get; private set; } = FlashBoardEventSource.Unknown;

	/// <summary>
	/// Fired when the called state changes (via user input or session sync).
	/// </summary>
	public event EventHandler<FlashBoardCalledChangedEventArgs>? IsCalledChanged;

	public FlashBoardNumber(int number, FlashBoardGroup parent)
	{
		Number = number;
		Parent = parent;
	}

	public void SetCalled(bool called, FlashBoardEventSource source)
	{
		if (IsCalled == called)
			return;

		bool oldValue = IsCalled;

		IsCalled = called;
		SourceTag = source;

		IsCalledChanged?.Invoke(this,
			new FlashBoardCalledChangedEventArgs(
				source: this,
				oldValue: oldValue,
				newValue: IsCalled,
				sourceTag: source));
	}
}
