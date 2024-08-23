using MauiStyler.App.Models;

namespace MauiStyler.App.Tools;

public class GimpPaletteHelper
{
    public static ColorPalette[] ReadGimpPalette(string[] filePaths)
    {
        List<ColorPalette> palettes = [];

        foreach (var item in filePaths)
        {
            ColorPalette palette = new();
            List<Color> colorsList = [];
            var lines = File.ReadAllLines(item);
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
                else if (line.StartsWith("#"))
                {
                    continue;
                }
                else if (line.Trim().Length == 0)
                {
                    isColorSection = true;
                }
                else if (isColorSection)
                {
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        if (int.TryParse(parts[0], out int r) &&
                            int.TryParse(parts[1], out int g) &&
                            int.TryParse(parts[2], out int b))
                        {
                            colorsList.Add(Color.FromRgb(r, g, b));
                        }
                    }

                }

            }

            palette.ColorsList = [.. colorsList];
        }

        return [.. palettes];
    }
}
