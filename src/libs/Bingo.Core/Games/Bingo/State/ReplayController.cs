namespace Bingo.Core.Games.Bingo.State;

public class ReplayController<T>
{
	private CancellationTokenSource? cts;

	public bool IsActive => cts?.IsCancellationRequested == false;

	public event Action<T>? ReplayStep;
	public event EventHandler? ReplayCompleted;

	public async void Start(IEnumerable<T> sequence, int delayMs, CancellationToken? externalToken = null)
	{
		Cancel();
		cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken ?? CancellationToken.None);

		try
		{
			foreach (T? item in sequence)
			{
				cts.Token.ThrowIfCancellationRequested();
				ReplayStep?.Invoke(item);
				await Task.Delay(delayMs, cts.Token);
			}

			ReplayCompleted?.Invoke(this, EventArgs.Empty);
		}
		catch (OperationCanceledException) { }
	}

	public void Cancel()
	{
		if (IsActive)
			cts?.Cancel();
		cts?.Dispose();
		cts = null;
	}
}
