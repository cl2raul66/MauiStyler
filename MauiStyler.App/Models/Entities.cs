using LiteDB;
using MauiStyler.App.Tools;

namespace MauiStyler.App.Models;

public class ColorStyle
{
    public string? Name { get; set; }
    public Color? Value { get; set; }
    public string? Tag { get; set; }
    public ColorScheme Scheme { get; set; }
}

public class StyleTemplate
{
    public ObjectId? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ColorStyle[]? ColorStyles { get; set; }
    public bool IsCustomTemplate { get; set; }
}

public class ColorPalette
{
    public string? Name { get; set; }
    public Dictionary<string, Color>? ColorsList { get; set; }
}
