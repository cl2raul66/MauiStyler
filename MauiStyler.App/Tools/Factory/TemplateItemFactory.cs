using MauiStyler.App.Models;

namespace MauiStyler.App.Tools;

public class TemplateItemFactory
{
    public static TemplateItem Create(StyleTemplate styleTemplate)
    {
        var templateItem = new TemplateItem
        {
            // Mapeo de campos simples
            Id = styleTemplate.Id?.ToString(),
            Name = styleTemplate.Name,
            Description = styleTemplate.Description,
            IsCustomTemplate = styleTemplate.IsCustomTemplate
        };

        // Mapeo de los colores
        if (styleTemplate.ColorStyles is not null)
        {
            templateItem.DefaultColorsStyle = styleTemplate.ColorStyles
                .Where(x => x.Tag == "PRINCIPAL" && x.Scheme == ColorScheme.Light)
                .Select(itemColor => itemColor.Value)
                .ToArray()!;

            templateItem.DarkColorsStyle = styleTemplate.ColorStyles
                .Where(x => x.Tag == "PRINCIPAL" && x.Scheme == ColorScheme.Dark)
                .Select(itemColor => itemColor.Value)
                .ToArray()!;
        }

        return templateItem;
    }
}

