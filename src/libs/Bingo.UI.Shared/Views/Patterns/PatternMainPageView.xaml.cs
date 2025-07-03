using System.Windows.Input;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class PatternMainPageView : ContentView
{
	public ICommand CloseCommand { get; }

	public PatternMainPageView()
	{
		InitializeComponent();

		CloseCommand = new Command(async () =>
		{
			await Navigation.PopModalAsync(); // Dismiss the modal
		});

		BindingContext = new { page = this };
	}
}