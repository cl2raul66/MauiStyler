using CommunityToolkit.Maui.Storage;

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

    //public static string DocumentsFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static string CachePath => FileSystem.Current.CacheDirectory;

    public static async Task DeleteFilesAndDirectoriesAsync(string[] paths)
    {
        foreach (var path in paths)
        {
            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
            else if (Directory.Exists(path))
            {
                await Task.Run(() => Directory.Delete(path, true));
            }
        }
    }

    public static async Task ExportTemplate(string[] src)
    {
        var folderResult = await FolderPicker.Default.PickAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        if (folderResult.IsSuccessful)
        {
            foreach (var item in src)
            {
                using Stream inputStream = File.OpenRead(item);
                string fileName = Path.GetFileName(item);
                string destinationPath = Path.Combine(folderResult.Folder!.Path, fileName);
                using FileStream outputStream = File.Create(destinationPath);
                await inputStream.CopyToAsync(outputStream);
            }
        }
    }

    public static async Task<IEnumerable<FileResult>> OpenFileGPL()
    {
        var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".gpl", ".gpl" } }, // file extension
                });

        PickOptions options = new()
        {
            PickerTitle = "Por favor seleccione uno o varios ficheros de paleta de GIMP",
            FileTypes = customFileType,
        };

        try
        {
            var result = await FilePicker.Default.PickMultipleAsync(options);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return [];
    }
}
