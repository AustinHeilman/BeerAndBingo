using Bingo.Core.FlashBoard;
using Bingo.Services.FlashBoard;

namespace Bingo.Tests.Services.FlashBoard;

public class FlashBoardServiceTests
{
	[Fact]
	public void NewGame_ShouldClearCalledNumbersAndHistory()
	{
		FlashBoardService service = new();
		service.CallNumber(10);
		service.CallNumber(25);

		service.NewGame();

		Assert.Empty(service.CalledNumbers);
		Assert.Empty(service.ActionHistory);
	}

	[Fact]
	public void CallNumber_ShouldAddToCalledNumbers_AndTrackHistory()
	{
		FlashBoardService service = new();

		service.CallNumber(10);
		service.CallNumber(20);

		Assert.Contains(10, service.CalledNumbers);
		Assert.Contains(20, service.CalledNumbers);

		List<(string Action, int Number)> history = service.ActionHistory.ToList();
		Assert.Equal(2, history.Count);
		Assert.Equal(("Call", 10), history[0]);
		Assert.Equal(("Call", 20), history[1]);
	}

	[Fact]
	public void UncallNumber_ShouldRemoveFromCalledNumbers_AndTrackHistory()
	{
		FlashBoardService service = new();

		service.CallNumber(33);
		service.UncallNumber(33);

		Assert.DoesNotContain(33, service.CalledNumbers);

		List<(string Action, int Number)> history = service.ActionHistory.ToList();
		Assert.Equal(("Call", 33), history[0]);
		Assert.Equal(("Uncall", 33), history[1]);
	}

	[Fact]
	public void GetAvailableNumbers_ShouldExcludeCalledNumbers()
	{
		FlashBoardService service = new();
		service.CallNumber(5);
		service.CallNumber(10);

		List<int> available = service.GetAvailableNumbers().ToList();

		Assert.DoesNotContain(5, available);
		Assert.DoesNotContain(10, available);
		Assert.Equal(FlashBoardConfig.TotalNumbers - 2, available.Count);
	}

	[Fact]
	public void GetAvailableNumbers_WithScope_ShouldOnlyIncludeLettersAndUncalled()
	{
		FlashBoardService service = new();
		service.CallNumber(1);   // B
		service.CallNumber(61);  // O

		List<int> scoped = service.GetAvailableNumbers(new[] { 'B', 'O' }).ToList();

		Assert.DoesNotContain(1, scoped);
		Assert.DoesNotContain(61, scoped);
		Assert.All(scoped, n =>
		{
			char letter = FlashBoardConfig.GetLetterForNumber(n);
			Assert.True(letter == 'B' || letter == 'O');
		});
	}

	[Fact]
	public void PickRandomAvailableNumber_ShouldReturnNull_WhenEmpty()
	{
		FlashBoardService service = new();

		for (int i = 1; i <= FlashBoardConfig.TotalNumbers; i++)
			service.CallNumber(i);

		int? pick = service.PickRandomAvailableNumber();
		Assert.Null(pick);
	}

	[Fact]
	public void PickRandomAvailableNumber_ShouldReturnNumber_WhenAvailable()
	{
		FlashBoardService service = new();

		int? pick = service.PickRandomAvailableNumber();
		Assert.NotNull(pick);
		Assert.InRange(pick.Value, 1, 75);
	}
}
