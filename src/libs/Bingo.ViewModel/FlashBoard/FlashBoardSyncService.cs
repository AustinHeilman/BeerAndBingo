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

		_session.ItemCalled += (_, _) => SyncFromSession();
		_session.UndoPerformed += (_, _) => SyncFromSession();
		_session.RedoPerformed += (_, _) => SyncFromSession();
		_session.NewGameStarted += (_, _) => _board.UpdateCalled(Array.Empty<int>());
		_session.ItemUncalled += (_, _) => SyncFromSession();

		SyncFromSession();
	}

	private void SyncFromSession() =>
		_board.UpdateCalled(_session.CalledItems);

	public void UpdateCalled(IEnumerable<int> calledNumbers)
	{
		_board.UpdateCalled(calledNumbers);
	}
}
