using Bingo.Core.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
namespace Bingo.UI.Shared.Views.Patterns;

// Use ValueChangedMessage or a plain object if MessageBase is not available
public class CloseCreatePatternMessage { }

public partial class CreatePatternView : ContentView
{
	public ICommand NoOpCommand { get; } = new Command(() => { });

	public string PatternName { get; set; } = "";
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public Action? RequestClose { get; set; }

	private HashSet<PatternCell> _editableCells = BingoPattern.EmptyPattern.Cells;
	public HashSet<PatternCell> EditableCells
	{
		get => _editableCells;
		set
		{
			if (_editableCells != value)
			{
				_editableCells = value;
				OnPropertyChanged(nameof(EditableCells));
			}
		}
	}

	public CreatePatternView()
	{
		InitializeComponent();

		SaveCommand = new Command(() =>
		{
			// TODO: save new BingoPattern with PatternName and current EditableCells
		});

		CancelCommand = new Command(() =>
		{
			EditableCells = new HashSet<PatternCell>(BingoPattern.EmptyPattern.Cells);
			WeakReferenceMessenger.Default.Send(new CloseCreatePatternMessage());
		});

		BindingContext = this;
	}
}

