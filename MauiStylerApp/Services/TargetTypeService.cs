using System.Reflection;
using System.Text.Json;

namespace MauiStylerApp.Services;

public interface ITargetTypeService
{
    VisualElement? GetTargetTypeInstance(string targetTypeName);
    IEnumerable<string> GetTargetTypeNames();
}

public class TargetTypeService : ITargetTypeService
{
    HashSet<string>? _targetTypes;

    public TargetTypeService()
    {
        Task.Run(LoadTargetTypes);
    }

    async Task LoadTargetTypes()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("TargetTypes.json");
        if (stream is null)
        {
            return;
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var jsonDocument = JsonDocument.Parse(json);
        _targetTypes = [..jsonDocument.RootElement.GetProperty("TargetTypes").EnumerateArray()
            .Select(element => element.GetString())
            .Where(s => !string.IsNullOrEmpty(s))];
    }

    public IEnumerable<string> GetTargetTypeNames()
    {
        return _targetTypes ?? [];
    }

    public VisualElement? GetTargetTypeInstance(string targetTypeName)
    {
        if (_targetTypes is null || !_targetTypes.Contains(targetTypeName))
        {
            return null;
        }

        Type? type = Type.GetType($"Microsoft.Maui.Controls.{targetTypeName}, Microsoft.Maui.Controls")
            ?? Type.GetType($"Microsoft.Maui.Controls.{targetTypeName}");

        if (type is null || !typeof(VisualElement).IsAssignableFrom(type))
        {
            return null;
        }

        return Activator.CreateInstance(type) as VisualElement;
    }
}
