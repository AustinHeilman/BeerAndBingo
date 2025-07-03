using Bingo.Core.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
namespace Bingo.UI.Shared.Views.Patterns;

// Use ValueChangedMessage or a plain object if MessageBase is not available
public class CloseCreatePatternMessage { }

public partial class CreatePatternView : ContentView
{
	public string PatternName { get; set; } = "";
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public Action? RequestClose { get; set; }
	public HashSet<PatternCell> EditableCells { get; } = BingoPattern.EmptyPattern.Cells;

	public CreatePatternView()
	{
		InitializeComponent();

		SaveCommand = new Command(() =>
		{
			// TODO: save new BingoPattern with PatternName and current EditableCells
		});

		CancelCommand = new Command(() =>
		{
			WeakReferenceMessenger.Default.Send(new CloseCreatePatternMessage());
		});

		BindingContext = this;
	}
}

