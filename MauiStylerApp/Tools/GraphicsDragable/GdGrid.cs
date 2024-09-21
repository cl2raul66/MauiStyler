using System.Collections.ObjectModel;

namespace MauiStylerApp.Tools;

public class GdGrid : IDrawable
{
    public ObservableCollection<IDrawable>? Children { get; set; }
    public string? Name { get; set; }
    public string? Type => "Grid";

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Color.FromRgb(253, 216, 53);
        canvas.StrokeColor = Color.FromRgba(253, 216, 53, 10);
        canvas.StrokeSize = 1.5f;
        canvas.DrawRectangle(dirtyRect);

        if (Children is not null || Children!.Count > 0)
        {
            foreach (var c in Children)
            {
                c.Draw(canvas, dirtyRect);
            }
        }
    }
}
