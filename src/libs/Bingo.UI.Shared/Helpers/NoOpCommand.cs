using System.Windows.Input;

namespace Bingo.UI.Shared.Helpers;

public class NoOpCommand : ICommand
{
	public static readonly ICommand Instance = new NoOpCommand();
	public event EventHandler? CanExecuteChanged;
	public bool CanExecute(object? parameter) => true;
	public void Execute(object? parameter) { }
}