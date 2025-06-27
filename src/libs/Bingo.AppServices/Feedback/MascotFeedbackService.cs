using Bingo.Services.Feedback;

namespace Bingo.AppServices.Feedback;

public class MascotFeedbackService : IMascotFeedbackService
{
	public string GetResponseForInvalidPattern()
		=> "That doesn’t look like a bingo, pal 🐾 Check again!";

	public string GetResponseForSuccessfulWin(string patternName)
		=> $"Bingo confirmed! That’s a solid {patternName} win! 🎉";
}
