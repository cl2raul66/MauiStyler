using MauiStylerApp.Models;

namespace MauiStylerApp.Tools;

public class GimpPaletteHelper
{
    public static ColorPalette[] ReadGimpPalette(string[] filePaths)
    {
        List<ColorPalette> palettes = [];

        foreach (var item in filePaths)
        {
            ColorPalette palette = new();
            var lines = File.ReadAllLines(item);
            int unnamedColorCount = 1;
            bool isColorSection = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("GIMP Palette"))
                {
                    continue;
                }
                else if (line.StartsWith("Name:"))
                {
                    palette.Name = line.Substring(5).Trim();
                }
                else if (line.StartsWith("Columns:"))
                {
                    continue; // Ignoramos la línea de columnas si no la necesitamos
                }
                else if (line.StartsWith("#"))
                {
                    // Ignoramos los comentarios
                    continue;
                }
                else if (!isColorSection)
                {
                    // La primera línea no vacía y no comentario marca el inicio de la sección de colores
                    isColorSection = true;
                }

                if (isColorSection && !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                {
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        if (int.TryParse(parts[0], out int r) &&
                            int.TryParse(parts[1], out int g) &&
                            int.TryParse(parts[2], out int b))
                        {
                            string colorName = parts.Length > 3 ? string.Join(" ", parts, 3, parts.Length - 3) : $"Color {unnamedColorCount++}";
                            palette.ColorsList ??= [];
                            palette.ColorsList[colorName] = Color.FromRgb(r, g, b);
                            //palette.ColorsList.TryAdd(colorName, Color.FromRgb(r, g, b));
                        }
                    }
                }
            }
            palettes.Add(palette);
        }

        return [.. palettes];
    }
}
