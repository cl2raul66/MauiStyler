namespace MauiStylerApp.Tools;

public interface IContrastHelper
{
    public bool CheckingTextLess18PT(Color foreground, Color background, WCAG2_1Levels level);
    public bool CheckingTextGreater18PT(Color foreground, Color background, WCAG2_1Levels level);
    public bool CheckingActionableIconsAndCharts(Color foreground, Color background, WCAG2_1Levels level);
}

public class ContrastHelper : IContrastHelper
{
    public bool CheckingActionableIconsAndCharts(Color foreground, Color background, WCAG2_1Levels level)
    {
        double contrastRatio = CalculateContrastRatio(foreground, background);
        return level == WCAG2_1Levels.AA ? contrastRatio >= 4.5 : contrastRatio >= 7.0;
    }

    public bool CheckingTextGreater18PT(Color foreground, Color background, WCAG2_1Levels level)
    {
        double contrastRatio = CalculateContrastRatio(foreground, background);
        return level == WCAG2_1Levels.AA ? contrastRatio >= 3.0 : contrastRatio >= 4.5;
    }

    public bool CheckingTextLess18PT(Color foreground, Color background, WCAG2_1Levels level)
    {
        double contrastRatio = CalculateContrastRatio(foreground, background);
        return level == WCAG2_1Levels.AA ? contrastRatio >= 3.0 : contrastRatio >= 4.5;
    }

    #region EXTRA
    double CalculateContrastRatio(Color foreground, Color background)
    {
        double luminance1 = CalculateLuminance(foreground);
        double luminance2 = CalculateLuminance(background);
        return (Math.Max(luminance1, luminance2) + 0.05) / (Math.Min(luminance1, luminance2) + 0.05);
    }

    double CalculateLuminance(Color color)
    {
        double r = color.Red / 255.0;
        double g = color.Green / 255.0;
        double b = color.Blue / 255.0;

        r = r <= 0.03928 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
        g = g <= 0.03928 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
        b = b <= 0.03928 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }
    #endregion
}
