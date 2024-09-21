using MauiStylerApp.Models;

namespace MauiStylerApp.Tools;

public class GdItemSelectedMessage
{
    public GdItem SelectedItem { get; }

    public GdItemSelectedMessage(GdItem selectedItem)
    {
        SelectedItem = selectedItem;
    }
}
