using Bingo.Core.Domain;
using Bingo.Core.FlashBoard;

namespace Bingo.ViewModel.FlashBoard;

public class FlashBoardSyncService
{
	private readonly GameSessionState<int> _session;
	private readonly FlashBoardObj _board;

	public FlashBoardObj Board => _board;

	public FlashBoardSyncService(GameSessionState<int> session)
	{
		_session = session;
		_board = new FlashBoardObj();

		_session.ItemCalled += (_, _) => Sync();
		_session.UndoPerformed += (_, _) => Sync();
		_session.RedoPerformed += (_, _) => Sync();
		_session.NewGameStarted += (_, _) => _board.UpdateCalled(Array.Empty<int>());
		_session.ItemUncalled += (_, _) => Sync();

		Sync();
	}

	private void Sync() =>
		_board.UpdateCalled(_session.CalledItems);
}
