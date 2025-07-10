using Bingo.Core.Patterns;
using Bingo.Services.Patterns;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows.Input;
namespace Bingo.UI.Shared.Views.Patterns;

// Use ValueChangedMessage or a plain object if MessageBase is not available
public class CloseCreatePatternMessage { }

public partial class CreatePatternView : ContentView
{
	public ICommand NoOpCommand { get; } = new Command(() => { });
	private readonly FilePatternRepository _repository;

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

	public CreatePatternView(FilePatternRepository repository)
	{
		InitializeComponent();
		_repository = repository;

		SaveCommand = new Command(async () =>
		{
			if (string.IsNullOrWhiteSpace(PatternName))
			{
				Debug.WriteLine("[CreatePatternView] Save aborted: PatternName is empty.");
				return;
			}

			BingoPattern pattern = new()
			{
				Name = PatternName.Trim(),
				Cells = new HashSet<PatternCell>(EditableCells)
			};

			await _repository.AddOrUpdatePattern(pattern);
			Debug.WriteLine($"[CreatePatternView] Saved pattern: {pattern.Name}");
			_repository.SetActivePattern(pattern);

			WeakReferenceMessenger.Default.Send(new CloseCreatePatternMessage());
		});


		CancelCommand = new Command(() =>
		{
			EditableCells = new HashSet<PatternCell>(BingoPattern.EmptyPattern.Cells);
			WeakReferenceMessenger.Default.Send(new CloseCreatePatternMessage());
		});

		BindingContext = this;
	}
}

