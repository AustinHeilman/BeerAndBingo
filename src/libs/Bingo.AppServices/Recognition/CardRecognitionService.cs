using Bingo.Services.Recognition;

namespace Bingo.AppServices.Recognition;

public class CardRecognitionService : ICardRecognitionService
{
	public async Task<HashSet<(int Row, int Col)>> RecognizeStampedCellsAsync(Stream image)
	{
		// Placeholder for ML.NET + OCR logic
		await Task.Delay(100); // Simulate async
		return new HashSet<(int, int)>();
	}
}
