namespace MauiStylerApp.Models;

public class TemplateItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Color[]? DefaultColorsStyle { get; set; }
    public Color[]? DarkColorsStyle { get; set; }
    public bool IsCustomTemplate { get; set; }
}

public class ColorStyleGroup : List<ColorStyle>
{
    public string? Key { get; private set; }

    public ColorStyleGroup(string nameGroup, IEnumerable<ColorStyle> colorStyles) : base (colorStyles)
    {
        Key = nameGroup;
    }

    public ColorStyleGroup(string nameGroup, ColorStyle[] colorStyles) : base (colorStyles)
    {
        Key = nameGroup;
    }

    public ColorStyleGroup(string nameGroup, List<ColorStyle> colorStyles) : base (colorStyles)
    {
        Key = nameGroup;
    }
}

public class ColorPaletteItem
{
    public string? Name { get; set; }
    public int Length { get; set; }
}
