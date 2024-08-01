using MauiStyler.App.Models;
using System.Xml.Linq;

namespace MauiStyler.App.Services;

public interface IColorStyleService
{
    string[] Sections { get; }
    bool GenerateColorTemplate(ItemColor[] principalsColors, ItemColor[] semanticsColors, ItemColor[] neutralsColors);
    Dictionary<string, ItemColor[]> LoadSelectedTemplate();
}

public class ColorStyleService : IColorStyleService
{
    XElement? mauiColorDocument;
    XElement? androidColorDocument;
    string cacheDir = FileSystem.Current.CacheDirectory;

    public string[] Sections => ["PRINCIPAL", "SEMANTIC", "NEUTRAL"];

    //public Dictionary<string, ItemColor[]> LoadDefaultTemplate()
    //{

    //}

    public Dictionary<string, ItemColor[]> LoadSelectedTemplate()
    {
        var colorDictionary = new Dictionary<string, ItemColor[]>();

        Task androidTask = Task.Run(() =>
        {
            string androidColorPath = Path.Combine(cacheDir, "colors.xml");
            androidColorDocument = XElement.Load(androidColorPath);

            var androidColors = androidColorDocument.Elements("color")
            .Select(c => new ItemColor
            {
                Name = c.Attribute("name")?.Value,
                Value = Color.Parse(c.Value)
            })
            .ToArray();

            colorDictionary.Add("Android", androidColors);
        });

        Task mauiTask = Task.Run(() =>
        {
            string mauiColorPath = Path.Combine(cacheDir, "Colors.xaml");
            mauiColorDocument = XElement.Load(mauiColorPath);

            var mauiColors = new Dictionary<string, ItemColor[]>();
            string currentSection = "";

            foreach (var node in mauiColorDocument.Nodes())
            {
                if (node is XComment comment)
                {
                    currentSection = comment.Value.Trim();
                }
                else if (node is XElement element && element.Name.LocalName == "Color")
                {
                    var color = new ItemColor
                    {
                        Name = element.Attribute("{http://schemas.microsoft.com/winfx/2009/xaml}Key")?.Value,
                        Value = Color.Parse(element.Value)
                    };

                    if (mauiColors.ContainsKey(currentSection))
                    {
                        mauiColors[currentSection] = [.. mauiColors[currentSection], color];
                    }
                    else
                    {
                        mauiColors[currentSection] = [color];
                    }
                }
            }

            foreach (var pair in mauiColors)
            {
                colorDictionary.Add(pair.Key, pair.Value);
            }
        });

        Task.WaitAll(androidTask, mauiTask);

        return colorDictionary;
    }

    public bool GenerateColorTemplate(ItemColor[] principalsColors, ItemColor[] semanticsColors, ItemColor[] neutralsColors)
    {
        if (principalsColors.Length > 0 && semanticsColors.Length > 0 && neutralsColors.Length > 0)
        {
            bool result = false;
            Task androidTask = Task.Run(() =>
            {
                androidColorDocument = new XElement("resources");
                foreach (var pc in principalsColors)
                {
                    string name = pc.Name switch
                    {
                        "Secondary" => "colorPrimaryDark",
                        "Accent" => "colorAccent",
                        _ => "colorPrimary"
                    };
                    androidColorDocument.Add(new XElement("color", new XAttribute("name", name), pc.Value!.ToHex()));
                }

                string androidColor = Path.Combine(cacheDir, "colors.xml");

                androidColorDocument.Save(androidColor);

                result = File.Exists(androidColor);
            });

            Task mauiTask = Task.Run(() =>
            {
                XNamespace xmlns = "http://schemas.microsoft.com/dotnet/2021/maui";
                XNamespace xmlnsx = "http://schemas.microsoft.com/winfx/2009/xaml";

                mauiColorDocument = new XElement(xmlns + "ResourceDictionary",
                    new XAttribute(XNamespace.Xmlns + "x", xmlnsx.NamespaceName));

                // Principals Colors
                mauiColorDocument.Add(new XComment("PRINCIPAL"));
                foreach (var pc in principalsColors!)
                {
                    mauiColorDocument.Add(new XElement(xmlns + "Color", new XAttribute(xmlnsx + "Key", pc.Name!), pc.Value!.ToHex()));
                }

                // Semantics Colors
                mauiColorDocument.Add(new XComment("SEMANTIC"));
                foreach (var sc in semanticsColors!)
                {
                    mauiColorDocument.Add(new XElement(xmlns + "Color", new XAttribute(xmlnsx + "Key", sc.Name!), sc.Value!.ToHex()));
                }

                // Neutrals Colors
                mauiColorDocument.Add(new XComment("NEUTRAL"));
                foreach (var nc in neutralsColors!)
                {
                    mauiColorDocument.Add(new XElement(xmlns + "Color", new XAttribute(xmlnsx + "Key", nc.Name!), nc.Value!.ToHex()));
                }


                string mauiFilePath = Path.Combine(cacheDir, "Colors.xaml");
                using (var writer = new StreamWriter(mauiFilePath))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    writer.WriteLine("<?xaml-comp compile=\"true\" ?>");
                    writer.Write(mauiColorDocument.ToString());
                }

                result = File.Exists(mauiFilePath);
            });

            Task.WaitAll(androidTask, mauiTask);

            androidColorDocument = null;
            mauiColorDocument = null;

            return result;
        }

        return false;
    }
}
