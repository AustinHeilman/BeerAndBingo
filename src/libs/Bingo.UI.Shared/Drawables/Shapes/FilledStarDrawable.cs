public class FilledStarDrawable : StarDrawable
{
	public Color FillColor { get; set; } = Colors.Gold;

	public override void Draw(ICanvas canvas, RectF dirtyRect)
	{
		// Base path logic reused
		float cx = dirtyRect.Center.X;
		float cy = dirtyRect.Center.Y;
		float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) * 0.4f;

		PathF path = new();
		for (int i = 0; i < 10; i++)
		{
			double angle = Math.PI / 2 + i * Math.PI / 5;
			float r = (i % 2 == 0) ? radius : radius * 0.4f;
			float x = cx + (float)(r * Math.Cos(angle));
			float y = cy - (float)(r * Math.Sin(angle));
			if (i == 0)
				path.MoveTo(x, y);
			else
				path.LineTo(x, y);
		}
		path.Close();

		canvas.FillColor = FillColor;
		canvas.FillPath(path);

		base.Draw(canvas, dirtyRect); // Reuse stroke logic
	}
}
