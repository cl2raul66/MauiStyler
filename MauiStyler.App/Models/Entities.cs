using LiteDB;

namespace MauiStyler.App.Models;

public class ItemColor
{
    public string? Name { get; set; }
    public Color? Value { get; set; }
}

public class PrincipalStyle
{
    public ItemColor[]? DefaultColorsStyle { get; set; }
    public ItemColor[]? DarkColorsStyle { get; set; }
}

public class SemanticStyle
{
    public ItemColor[]? DefaultColorsStyle { get; set; }
    public ItemColor[]? DarkColorsStyle { get; set; }
}

public class NeutralStyle
{
    public ItemColor[]? DefaultColorsStyle { get; set; }
    public ItemColor[]? DarkColorsStyle { get; set; }
}

public class StyleTemplate
{
    public ObjectId? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PrincipalStyle? PrincipalStyle { get; set; }
    public SemanticStyle? SemanticStyle { get; set; }
    public NeutralStyle? NeutralStyle { get; set; }
    public bool IsCustomTemplate { get; set; }
}

public class ColorPalette
{
    public string? Name { get; set; }
    public Dictionary<string, Color>? ColorsList { get; set; }
}
