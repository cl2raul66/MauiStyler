namespace MauiStyler.App.Models;

public class TemplateItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Color[]? DefaultColorsStyle { get; set; }
    public Color[]? DarkColorsStyle { get; set; }
    public bool IsCustomTemplate { get; set; }
}

public class ColorPaletteItem
{
    public string? Name { get; set; }
    public int Length { get; set; }
}
