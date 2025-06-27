public static class ViewExtensions
{
	public static Task ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> onUpdate, uint length = 250)
	{
		TaskCompletionSource<bool> taskCompletionSource = new();

		Animation animation = new(d =>
		{
			double r = fromColor.Red + (toColor.Red - fromColor.Red) * d;
			double g = fromColor.Green + (toColor.Green - fromColor.Green) * d;
			double b = fromColor.Blue + (toColor.Blue - fromColor.Blue) * d;
			double a = fromColor.Alpha + (toColor.Alpha - fromColor.Alpha) * d;
			onUpdate(new Color(
						(float)(fromColor.Red + (toColor.Red - fromColor.Red) * d),
						(float)(fromColor.Green + (toColor.Green - fromColor.Green) * d),
						(float)(fromColor.Blue + (toColor.Blue - fromColor.Blue) * d),
						(float)(fromColor.Alpha + (toColor.Alpha - fromColor.Alpha) * d)
				   ));
		});

		animation.Commit(self, "ColorTo", 16, length, Easing.Linear, (v, c) => taskCompletionSource.SetResult(true));

		return taskCompletionSource.Task;
	}
}
