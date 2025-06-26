using Bingo.AppServices.Feedback;

namespace Bingo.AppServices.Tests.Feedback;

public class MascotFeedbackServiceTests
{
	private readonly MascotFeedbackService _service = new();

	[Fact]
	public void GetResponseForInvalidPattern_ReturnsSnark()
	{
		string response = _service.GetResponseForInvalidPattern();
		Assert.Contains("bingo", response, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public void GetResponseForSuccessfulWin_ReferencesPatternName()
	{
		string response = _service.GetResponseForSuccessfulWin("Heart");
		Assert.Contains("Heart", response);
		Assert.Contains("Bingo", response, StringComparison.OrdinalIgnoreCase);
	}
}
