public class StarDrawable : IStar
{
    public virtual float StrokeSize { get; set; } = 1f;
    public virtual Color StrokeColor { get; set; } = Colors.Black;

    public virtual void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float cx = dirtyRect.Center.X;
        float cy = dirtyRect.Center.Y;
        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) * 0.4f;

        var path = new PathF();
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

        canvas.StrokeColor = StrokeColor;
        canvas.StrokeSize = StrokeSize;
        canvas.DrawPath(path);
    }
}
