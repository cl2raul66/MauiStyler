
namespace MauiStyler.App.Tools;

public class FileHelper
{
    static readonly string DIR_DB = Path.Combine(AppContext.BaseDirectory, "Data");

    public static string GetFileDbPath(string db_filename)
    {
        if (!Directory.Exists(DIR_DB))
        {
            Directory.CreateDirectory(DIR_DB);
        }

        return Path.Combine(DIR_DB, $"{db_filename}.db");
    }
}
