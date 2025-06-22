namespace Bingo.Services.Feedback;

public interface IMascotFeedbackService
{
    string GetResponseForInvalidPattern();
    string GetResponseForSuccessfulWin(string patternName);
}
