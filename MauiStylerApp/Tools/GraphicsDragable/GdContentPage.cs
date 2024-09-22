using CommunityToolkit.Mvvm.Messaging;
using MauiStylerApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MauiStylerApp.Tools;

public class GdContentPage : GraphicsView
{
    public static readonly BindableProperty GdItemsProperty =
        BindableProperty.Create(
            nameof(GdItems),
            typeof(ObservableCollection<GdItem>),
            typeof(GdContentPage),
            null,
            propertyChanged: (bindable, oldValue, newValue) => {
                var canvas = (GdContentPage)bindable;
                if (oldValue is ObservableCollection<GdItem> oldCollection)
                {
                    oldCollection.CollectionChanged -= canvas.OnGdContentPageCollectionChanged; ;
                }
                if (newValue is ObservableCollection<GdItem> newCollection)
                {
                    newCollection.CollectionChanged += canvas.OnGdContentPageCollectionChanged;
                }
                canvas.Invalidate();
            });

    public ObservableCollection<GdItem>? GdItems
    {
        get => (ObservableCollection<GdItem>)GetValue(GdItemsProperty);
        set => SetValue(GdItemsProperty, value);
    }

    public GdContentPage()
    {
        Drawable = new GdContentPageDrawable(this);
        StartInteraction += GdContentPage_StartInteraction;
    }

    private void GdContentPage_StartInteraction(object? sender, TouchEventArgs e)
    {
        var point = e.Touches[0];
        var selectedItem = FindRectangleAt(point, [..GdItems]);
        if (selectedItem is not null)
        {
            WeakReferenceMessenger.Default.Send(new GdItemSelectedMessage(selectedItem));
        }
        Invalidate();
    }

    private void OnGdContentPageCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Invalidate();
    }

    private GdItem? FindRectangleAt(Point point, IEnumerable<GdItem> items)
    {
        foreach (var item in items.Reverse())
        {
            var bounds = new Rect(
                Width * item.Bounds.X,
                Height * item.Bounds.Y,
                Width * item.Bounds.Width,
                Height * item.Bounds.Height
            );

            if (bounds.Contains(point))
            {
                var childRect = FindRectangleAt(point, item.Children);
                return childRect ?? item;
            }
        }

        return null;
    }    

    class GdContentPageDrawable : IDrawable
    {
        private readonly GdContentPage _canvas;

        public GdContentPageDrawable(GdContentPage canvas)
        {
            _canvas = canvas;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_canvas.GdItems is not null)
            {
                foreach (var item in _canvas.GdItems)
                {
                    DrawRectangle(canvas, item, dirtyRect);
                }
            }
        }

        private void DrawRectangle(ICanvas canvas, GdItem item, RectF parentBounds)
        {
            var bounds = new RectF(
                parentBounds.Left + item.Bounds.X * parentBounds.Width,
                parentBounds.Top + item.Bounds.Y * parentBounds.Height,
                item.Bounds.Width * parentBounds.Width,
                item.Bounds.Height * parentBounds.Height
            );

            canvas.FillColor = item.FillColor;
            canvas.FillRectangle(bounds);

            canvas.StrokeColor = item.BorderColor;
            canvas.StrokeSize = 1.5f;
            if (item.IsSelected)
            {
                canvas.StrokeDashPattern = [5, 5];
            }
            else
            {
                canvas.StrokeDashPattern = null;
            }
            canvas.DrawRectangle(bounds);

            foreach (var child in item.Children)
            {
                DrawRectangle(canvas, child, bounds);
            }
        }
    }
}
