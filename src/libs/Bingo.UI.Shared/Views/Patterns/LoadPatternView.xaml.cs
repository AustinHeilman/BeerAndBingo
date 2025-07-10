using Bingo.Core.Patterns;
using Bingo.ViewModel.Patterns;
using Microsoft.Maui.Controls;

namespace Bingo.UI.Shared.Views.Patterns;

public partial class LoadPatternView : ContentView
{
    private bool _initialized = false;
    private readonly LoadPatternViewModel _viewModel;
    private readonly PatternRepositoryBase _repository;

    public LoadPatternView(LoadPatternViewModel viewModel, PatternRepositoryBase repository)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _repository = repository;
    }

    protected override async void OnParentSet()
    {
        base.OnParentSet();

        if (!_initialized && _repository is not null)
        {
            _initialized = true;
            try
            {
                await _viewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing patterns: {ex.Message}");
            }
        }
    }
}
