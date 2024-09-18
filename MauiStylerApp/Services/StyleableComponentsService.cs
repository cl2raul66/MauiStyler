// Ignore Spelling: Styleable

using System.Reflection;

namespace MauiStylerApp.Services;

public interface IStyleableComponentsService
{
    IEnumerable<Type> GetStyleableTypes();
}

public class StyleableComponentsService : IStyleableComponentsService
{
    public IEnumerable<Type> GetStyleableTypes()
    {
        var assembly = typeof(VisualElement).Assembly;
        var styleableTypes = new List<Type>();

        foreach (var type in assembly.GetTypes())
        {
            if (IsStyleableType(type))
            {
                styleableTypes.Add(type);
            }
        }

        return styleableTypes;
    }

    bool IsStyleableType(Type type)
    {
        // Verifica si el tipo es público
        if (!type.IsPublic)
            return false;

        // Verifica si el tipo hereda de VisualElement o implementa IVisualElementController
        if (!typeof(VisualElement).IsAssignableFrom(type) &&
            !typeof(IVisualElementController).IsAssignableFrom(type))
            return false;

        // Excluye tipos genéricos abiertos
        if (type.IsGenericTypeDefinition)
            return false;

        // Incluye tipos que tienen la propiedad Style o que heredan de tipos que la tienen
        var currentType = type;
        while (currentType != null && currentType != typeof(object))
        {
            var styleProperty = currentType.GetProperty("Style", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (styleProperty != null && styleProperty.PropertyType == typeof(Style))
                return true;
            currentType = currentType.BaseType;
        }

        return false;
    }
}
