using Bingo.Core.Games.Bingo.FlashBoard;

namespace Bingo.Core.Tests.Domain;

public class BingoBoardConfigTests
{
	private readonly FlashBoardConfig FlashBoardConfig = new();

	[Theory]
	[InlineData(0, 0, 1)]
	[InlineData(4, 0, 5)]
	[InlineData(0, 1, 6)]
	[InlineData(4, 1, 10)]
	public void GetNumberForCell_ShouldReturnExpectedValues(int row, int column, int expected)
	{
		int result = FlashBoardConfig.GetNumberForCell(row, column);
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData('B', 0)]
	[InlineData('I', 1)]
	[InlineData('N', 2)]
	[InlineData('G', 3)]
	[InlineData('O', 4)]
	public void GetIndexForBoardLetter_ShouldReturnExpectedIndex(char letter, int expectedIndex)
	{
		int index = FlashBoardConfig.GetIndexForLetter(letter);
		Assert.Equal(expectedIndex, index);
	}

	[Fact]
	public void GetIndexForBoardLetter_ShouldThrow_ForInvalidLetter()
	{
		Assert.Throws<ArgumentException>(() => FlashBoardConfig.GetIndexForLetter('Z'));
	}

	[Theory]
	[InlineData(0, 1, 15)]
	[InlineData(1, 16, 30)]
	[InlineData(4, 61, 75)]
	public void GetNumbersInBoardIndex_ShouldReturnExpectedRange(int index, int expectedStart, int expectedEnd)
	{
		List<int> range = FlashBoardConfig.GetNumbersInIndex(index).ToList();

		Assert.Equal(15, range.Count);
		Assert.Equal(expectedStart, range.First());
		Assert.Equal(expectedEnd, range.Last());
	}

	[Fact]
	public void GetNumbersInBoardIndex_ShouldThrow_ForOutOfRangeIndex()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => FlashBoardConfig.GetNumbersInIndex(-1));
		Assert.Throws<ArgumentOutOfRangeException>(() => FlashBoardConfig.GetNumbersInIndex(5));
	}

	[Theory]
	[InlineData('B', 1, 15)]
	[InlineData('O', 61, 75)]
	public void GetNumbersInBoardLetter_ShouldReturnExpectedRange(char letter, int expectedStart, int expectedEnd)
	{
		List<int> range = FlashBoardConfig.GetNumbersInLetter(letter).ToList();

		Assert.Equal(15, range.Count);
		Assert.Equal(expectedStart, range.First());
		Assert.Equal(expectedEnd, range.Last());
	}

	[Theory]
	[InlineData(1, 'B')]
	[InlineData(15, 'B')]
	[InlineData(16, 'I')]
	[InlineData(30, 'I')]
	[InlineData(31, 'N')]
	[InlineData(45, 'N')]
	[InlineData(75, 'O')]
	public void GetLetterForNumber_ShouldReturnExpectedLetter(int number, char expectedLetter)
	{
		char result = FlashBoardConfig.GetLetterForNumber(number);
		Assert.Equal(expectedLetter, result);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(76)]
	public void GetLetterForNumber_ShouldThrow_IfOutOfRange(int number)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => FlashBoardConfig.GetLetterForNumber(number));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(75)]
	[InlineData(37)]
	public void IsValidNumber_ShouldReturnTrue_ForValidNumbers(int number)
	{
		Assert.True(FlashBoardConfig.IsValidNumber(number));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(76)]
	[InlineData(-5)]
	public void IsValidNumber_ShouldReturnFalse_ForInvalidNumbers(int number)
	{
		Assert.False(FlashBoardConfig.IsValidNumber(number));
	}
}
