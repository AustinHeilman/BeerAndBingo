namespace Bingo.Services.Recognition;

public interface ICardRecognitionService
{
	Task<HashSet<(int Row, int Col)>> RecognizeStampedCellsAsync(Stream image);
}
