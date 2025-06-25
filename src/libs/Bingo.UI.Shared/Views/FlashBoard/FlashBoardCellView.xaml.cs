using Bingo.ViewModel.FlashBoard;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices; // for haptics

namespace Bingo.UI.Shared.Views.FlashBoard;

public partial class FlashBoardCellView : ContentView
{
    private int _animationToken = 0;
    private CancellationTokenSource? _animationTokenSource;

    public FlashBoardCellView()
    {
        InitializeComponent();
        this.BindingContextChanged += OnBound;
    }

    private void OnBound(object? sender, EventArgs e)
    {
        if (BindingContext is FlashBoardCellViewModel vm)
        {
            vm.PropertyChanged += async (s, args) =>
            {
                if (args.PropertyName == nameof(vm.IsCalled))
                {
                    await AnimateCalledStateAsync(vm.IsCalled, 2.0);
                }
            };
        }
    }

    public async Task AnimateCalledStateAsync(bool isCalled, double animationScale)
    {
        _animationTokenSource?.Cancel();
        var cts = new CancellationTokenSource();
        _animationTokenSource = cts;

        if (isCalled)
        {
            try
            {
                await AnimateInAsync(animationScale, cts.Token);
                //Diable for now - Causes crash: HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
            catch (OperationCanceledException) { /* Swallowed safely */ }
        }
        else
        {
            await AnimateOutAsync();
        }

		//StateSymbolOverlay.Text = isCalled ? "✔" : "✖";
		StateSymbolOverlay.Text = isCalled ? "" : "✖";
		await StateSymbolOverlay.FadeTo(1.0, 100);
		await Task.Delay(500);
		await StateSymbolOverlay.FadeTo(0.0, 200);
	}

    private async Task AnimateInAsync(double scale, CancellationToken token)
    {
        this.SetValue(Microsoft.Maui.Controls.Layout.ZIndexProperty, 1);

        Color shimmerColor = Colors.Goldenrod.WithAlpha(0.6f);
        Color settledColor = Application.Current?.Resources?.TryGetValue("FlashBoardCellBGColor_Called", out var color) == true && color is Color validColor
            ? validColor
            : Colors.Transparent;

        uint scaleUp = (uint)(150 * scale);
        uint fade = (uint)(120 * scale);
        uint settle = (uint)(200 * scale);
        uint scaleDown = (uint)(120 * scale);

        token.ThrowIfCancellationRequested();
        await CellBorder.ScaleTo(1.15, scaleUp, Easing.SinOut);

        token.ThrowIfCancellationRequested();
        await CellBorder.FadeTo(0.2, fade);
        await CellBorder.FadeTo(1.0, fade);
        await CellBorder.FadeTo(0.3, fade);
        await CellBorder.FadeTo(1.0, fade);

        token.ThrowIfCancellationRequested();
        await CellBorder.ColorTo(shimmerColor, settledColor, c => CellBorder.BackgroundColor = c, settle);

        token.ThrowIfCancellationRequested();
        await CellBorder.ScaleTo(1.0, scaleDown, Easing.SinIn);
        this.SetValue(Microsoft.Maui.Controls.Layout.ZIndexProperty, 0);
    }

    private async Task AnimateOutAsync()
    {
        _animationTokenSource?.Cancel();

        await CellBorder.ScaleTo(1.0, 100, Easing.SinIn);
        CellBorder.Opacity = 1.0;
        this.SetValue(Microsoft.Maui.Controls.Layout.ZIndexProperty, 0);

        if (Application.Current?.Resources?.TryGetValue("FlashBoardCellBGColor_Uncalled", out var fallback) == true &&
            fallback is Color reset)
        {
            CellBorder.BackgroundColor = reset;
        }
        else
        {
            CellBorder.BackgroundColor = Colors.Transparent;
        }
    }


    private async Task SafeAnimateAsync(int token, Func<Task> animationBlock)
    {
        await animationBlock();
        if (_animationToken != token)
            throw new OperationCanceledException("Animation superseded");
    }
}
