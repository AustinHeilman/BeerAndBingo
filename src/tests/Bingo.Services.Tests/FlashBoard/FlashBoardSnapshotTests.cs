using Bingo.Services.FlashBoard;

namespace Bingo.Services.Tests.FlashBoard;

public class FlashBoardSnapshotTests
{
	[Fact]
	public void Snapshot_ShouldContainAllCalledNumbers()
	{
		FlashBoardService service = new();
		service.CallNumber(5);
		service.CallNumber(7);

		Core.Domain.FlashBoard.FlashBoardSnapshot snapshot = service.GetSnapshot();

		Assert.Contains(5, snapshot.CalledNumbers);
		Assert.Contains(7, snapshot.CalledNumbers);
		Assert.Equal(2, snapshot.CalledNumbers.Count);
	}

	[Fact]
	public void Snapshot_ShouldBeIndependentCopy()
	{
		FlashBoardService service = new();
		service.CallNumber(30);
		Core.Domain.FlashBoard.FlashBoardSnapshot snapshot = service.GetSnapshot();

		service.UncallNumber(30);

		Assert.Contains(30, snapshot.CalledNumbers);
		Assert.DoesNotContain(30, service.CalledNumbers);
	}
}
