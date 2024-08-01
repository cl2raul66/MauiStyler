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
        if (styleTemplate.PrincipalStyle is not null)
        {
            if (styleTemplate.PrincipalStyle.DefaultColorsStyle is not null)
            {
                templateItem.DefaultColorsStyle = styleTemplate.PrincipalStyle.DefaultColorsStyle
                    .Select(itemColor => itemColor.Value)
                    .ToArray()!;
            }

            if (styleTemplate.PrincipalStyle.DarkColorsStyle is not null)
            {
                templateItem.DarkColorsStyle = styleTemplate.PrincipalStyle.DarkColorsStyle
                    .Select(itemColor => itemColor.Value)
                    .ToArray()!;
            }
        }

        return templateItem;
    }
}

