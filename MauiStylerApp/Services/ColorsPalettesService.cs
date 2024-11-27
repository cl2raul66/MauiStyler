using LiteDB;
using MauiStylerApp.Models;
using MauiStylerApp.Tools;
using System.Reflection;

namespace MauiStylerApp.Services;

public interface IColorsPalettesService : IDisposable
{
    void BeginTrans();
    void Commit();
    bool Delete(string name);
    IEnumerable<ColorPalette> GetAll();
    ColorPalette GetByName(string name);
    string Insert(ColorPalette entity);
    bool InsertMany(ColorPalette[] entities);
    void Rollback();
    bool Update(ColorPalette entity);
}

public class ColorsPalettesService : IColorsPalettesService
{
    readonly LiteDatabase db;
    readonly ILiteCollection<ColorPalette> collection;

    public ColorsPalettesService()
    {
        var cnx = new ConnectionString()
        {
            Filename = FileHelper.GetFileDbPath("ColorPalette")
        };

        BsonMapper mapper = new();

        mapper.RegisterType<Color>(
            serialize: (color) => color.ToArgbHex(),
            deserialize: (bson) => Color.Parse(bson.AsString)
        );

        db = new LiteDatabase(cnx, mapper);
        collection = db.GetCollection<ColorPalette>();

        collection.EnsureIndex(x => x.Name, true);

        if (collection.Count() == 0)
        {
            LoadCustomPalette();
        }
    }

    public void BeginTrans() => db.BeginTrans();

    public void Commit() => db.Commit();

    public void Rollback() => db.Rollback();

    public void Dispose() => db.Dispose();

    public IEnumerable<ColorPalette> GetAll() => collection.FindAll();

    public ColorPalette GetByName(string name) => collection.FindById(name);

    public string Insert(ColorPalette entity) => collection.Insert(entity).AsString;

    public bool InsertMany(ColorPalette[] entities)
    {
        db.BeginTrans();
        var result = collection.InsertBulk(entities) == entities.Length;
        if (!result)
        {
            db.Rollback();
        }
        db.Commit();
        return result;
    }

    public bool Update(ColorPalette entity) => collection.Update(entity);

    public bool Delete(string name) => collection.Delete(name);

    #region EXTRA
    private void LoadCustomPalette()
    {
        HashSet<KeyValuePair<string, Color>> allColors = [];
        var colorType = typeof(Colors);

        foreach (var field in colorType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.FieldType == typeof(Color))
            {
                var color = (Color)field.GetValue(null)!;
                allColors.Add(new(field.Name, color));
            }
        }

        if (allColors.Count != 0)
        {
            ColorPalette customPalette = new() { Name = "MAUI" };
            customPalette.ColorsList ??= [];

            foreach (var kvp in allColors)
            {
                customPalette.ColorsList[kvp.Key] = kvp.Value;
            }

            collection.Insert(customPalette);
        }
    }
    #endregion
}
