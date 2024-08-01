using LiteDB;
using MauiStyler.App.Models;
using MauiStyler.App.Tools;

namespace MauiStyler.App.Services;

public interface IStyleTemplateService
{
    bool Exist { get; }

    void BeginTrans();
    void Commit();
    bool Delete(ObjectId id);
    IEnumerable<StyleTemplate> GetAll();
    StyleTemplate? GetById(ObjectId id);
    string Insert(StyleTemplate entity);
    void Rollback();
    bool Update(StyleTemplate entity);
}

public class StyleTemplateService : IStyleTemplateService
{
    readonly LiteDatabase db;
    readonly ILiteCollection<StyleTemplate> collection;

    public StyleTemplateService()
    {
        var cnx = new ConnectionString()
        {
            Filename = FileHelper.GetFileDbPath("StyleTemplate")
        };

        BsonMapper mapper = new();

        mapper.RegisterType<Color>(
            serialize: (color) => color.ToArgbHex(),
            deserialize: (bson) => Color.Parse(bson.AsString)
        );

        db = new LiteDatabase(cnx, mapper);
        collection = db.GetCollection<StyleTemplate>();
    }

    public void BeginTrans() => db.BeginTrans();

    public void Commit() => db.Commit();

    public void Rollback() => db.Rollback();

    public bool Exist => collection.Count() > 0;

    public IEnumerable<StyleTemplate> GetAll() => collection.FindAll();

    public StyleTemplate? GetById(ObjectId id) => collection.FindById(id);

    public string Insert(StyleTemplate entity)
    {
        entity.Id = ObjectId.NewObjectId();
        return collection.Insert(entity).AsObjectId.ToString();
    }

    public bool Update(StyleTemplate entity) => collection.Update(entity);

    public bool Delete(ObjectId id) => collection.Delete(id);
}
