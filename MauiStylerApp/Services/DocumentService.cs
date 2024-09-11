using MauiStylerApp.Models;
using MauiStylerApp.Tools;
using System.Xml.Linq;
namespace MauiStylerApp.Services;

public interface IDocumentService
{
    string[] Sections { get; }

    Dictionary<string, string> GenerateColorTemplate(StyleTemplate template);
    Dictionary<string, ColorStyle[]> LoadSelectedTemplate();
}

public class DocumentService : IDocumentService
{
    XElement? mauiColorDocument;
    XElement? androidColorDocument;

    string cacheDir = FileHelper.CachePath;

    public string[] Sections => ["PRINCIPAL", "SEMANTIC", "NEUTRAL"];

    //public Dictionary<string, ItemColor[]> LoadDefaultTemplate()
    //{

    //}

    public Dictionary<string, ColorStyle[]> LoadSelectedTemplate()
    {
        var colorDictionary = new Dictionary<string, ColorStyle[]>();
        string generatePath = Path.Combine(cacheDir, "Generate");

        Task androidTask = Task.Run(() =>
        {
            string androidColorPath = Path.Combine(generatePath, "colors.xml");
            androidColorDocument = XElement.Load(androidColorPath);

            var androidColors = androidColorDocument.Elements("color")
            .Select(c => new ColorStyle
            {
                Name = c.Attribute("name")?.Value,
                Value = Color.Parse(c.Value)
            })
            .ToArray();

            colorDictionary.Add("Android", androidColors);
        });

        Task mauiTask = Task.Run(() =>
        {
            string mauiColorPath = Path.Combine(generatePath, "Colors.xaml");
            mauiColorDocument = XElement.Load(mauiColorPath);

            var mauiColors = new Dictionary<string, ColorStyle[]>();
            string currentSection = "";

            foreach (var node in mauiColorDocument.Nodes())
            {
                if (node is XComment comment)
                {
                    currentSection = comment.Value.Trim();
                }
                else if (node is XElement element && element.Name.LocalName == "Color")
                {
                    var color = new ColorStyle
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

    public Dictionary<string, string> GenerateColorTemplate(StyleTemplate template)
    {
        if (template is null)
        {
            return [];
        }

        Dictionary<string, string> result = new() { { "Android", string.Empty }, { "MAUI", string.Empty } };
        string generatePath = Path.Combine(cacheDir, "Generate");
        Directory.CreateDirectory(generatePath);
        if (Directory.Exists(generatePath))
        {
            var principalsColors = template.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light && x.Tag == "PRINCIPAL");
            var semanticsColors = template.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light && x.Tag == "SEMANTIC");
            var neutralsColors = template.ColorStyles!.Where(x => x.Scheme == ColorScheme.Light && x.Tag == "NEUTRAL");
            if (principalsColors is null && semanticsColors is null && neutralsColors is null)
            {
                return [];
            }

            Task androidTask = Task.Run(() =>
            {
                androidColorDocument = new XElement("resources");
                androidColorDocument.Add(new XElement("color", new XAttribute("name", "colorPrimary"), principalsColors!.First(x => x.Name == "Primary").Value!.ToArgbHex()));
                androidColorDocument.Add(new XElement("color", new XAttribute("name", "colorPrimaryDark"), principalsColors!.First(x => x.Name == "PrimaryDark").Value!.ToArgbHex()));
                androidColorDocument.Add(new XElement("color", new XAttribute("name", "colorAccent"), principalsColors!.First(x => x.Name == "Accent").Value!.ToArgbHex()));

                string androidColor = Path.Combine(generatePath, "colors.xml");

                androidColorDocument.Save(androidColor);

                if (File.Exists(androidColor))
                {
                    result["Android"] = androidColor;
                }
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

                string mauiFilePath = Path.Combine(generatePath, "Colors.xaml");
                using (var writer = new StreamWriter(mauiFilePath))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    writer.WriteLine("<?xaml-comp compile=\"true\" ?>");
                    writer.Write(mauiColorDocument.ToString());
                }

                if (File.Exists(mauiFilePath))
                {
                    result["MAUI"] = mauiFilePath;
                }
            });

            Task.WaitAll(androidTask, mauiTask);
        }
        androidColorDocument = null;
        mauiColorDocument = null;
        return result;
    }
}
