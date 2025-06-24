using Bingo.ViewModel.FlashBoard;
using Microsoft.Maui.Controls;

namespace Bingo.UI.Shared.Views.FlashBoard;

public partial class FlashBoardCellView : ContentView
{
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
                    // Change the multiplier here to adjust pacing (1.0 = normal)
                    await AnimateCalledStateAsync(vm.IsCalled, 2.0);
                }
            };
        }
    }

    public async Task AnimateCalledStateAsync(bool isCalled, double animationScale)
    {
        if (CellBorder == null) return;

        if (isCalled)
        {
            this.SetValue(Microsoft.Maui.Controls.Layout.ZIndexProperty, 1); // bring to front
            Color shimmerColor = Colors.Goldenrod.WithAlpha(0.6f);
            Color settledColor = (Color)Application.Current.Resources["FlashBoardCellBGColor_Called"];

            uint scaleUpTime = (uint)(150 * animationScale);
            uint fadeTime = (uint)(120 * animationScale);
            uint settleTime = (uint)(200 * animationScale);
            uint scaleDownTime = (uint)(120 * animationScale);

            await CellBorder.ScaleTo(1.15, scaleUpTime, Easing.SinOut);

            await CellBorder.FadeTo(0.2, fadeTime); // near blackout
            await CellBorder.FadeTo(1.0, fadeTime); // full flash
            await CellBorder.FadeTo(0.3, fadeTime); // low-glow echo
            await CellBorder.FadeTo(1.0, fadeTime); // final return

            await CellBorder.ColorTo(
                shimmerColor,
                settledColor,
                c => CellBorder.BackgroundColor = c,
                settleTime
            );

            await CellBorder.ScaleTo(1.0, scaleDownTime, Easing.SinIn);
            this.SetValue(Microsoft.Maui.Controls.Layout.ZIndexProperty, 0); // reset
        }
        else
        {
            CellBorder.Scale = 1.0;
            CellBorder.Opacity = 1.0;

            if (Application.Current.Resources.TryGetValue("FlashBoardCellBGColor_Uncalled", out object? fallback) && fallback is Color reset)
            {
                CellBorder.BackgroundColor = reset;
            }
            else
            {
                CellBorder.BackgroundColor = Colors.Transparent;
            }
        }
    }
}
