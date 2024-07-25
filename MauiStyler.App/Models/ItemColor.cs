namespace MauiStyler.App.Models;

//public class ColorStyle
//{
//    // Este es el color dominante en tu interfaz y es el que más se utiliza. Debería reflejar la personalidad de tu marca.
//    public ItemColor? Primary { get; set; }
//    // Este color complementa al color principal y se utiliza para resaltar áreas importantes y llamar la atención del usuario.
//    public ItemColor? Secondary { get; set; }
//    // Este color se utiliza en pequeñas cantidades para llamar la atención sobre detalles específicos.
//    public ItemColor? Accent { get; set; }

//    // Estos colores se utilizan para transmitir significado, como rojo para errores, verde para éxito, y amarillo para advertencias.
//    public ItemColor[]? SemanticColors { get; set; }
//    // Estos colores se utilizan para áreas de fondo y contenido secundario.
//    public ItemColor[]? NeutralColors { get; set; }

//    public ColorStyle()
//    {
//        Primary = new ItemColor() { Name = "Primary", Value = Color.Parse("#512BD4") };
//        Secondary = new ItemColor() { Name = "Secondary", Value = Color.Parse("#2B0B98") };
//        Accent = new ItemColor() { Name = "Accent", Value = Color.Parse("#2B0B98") };
//        SemanticColors = [
//            new ItemColor() { Name = "Error", Value = Color.Parse("#FF0000") },
//            new ItemColor() { Name = "Success", Value = Color.Parse("#00FF00") },
//            new ItemColor() { Name = "Warning", Value = Color.Parse("#FFFF00") },
//        ];
//        NeutralColors = [
//            new ItemColor() { Name = "Gray250", Value = Color.Parse("#E1E1E1") },
//            new ItemColor() { Name = "Gray500", Value = Color.Parse("#ACACAC") },
//            new ItemColor() { Name = "Gray750", Value = Color.Parse("#6E6E6E") }
//        ];
//    }
//}

public class ItemColor
{
    public string? Name { get; set; }
    public Color? Value { get; set; }
}
