namespace NoMercy.Server.MediaBrowser;

public class DirectoryScanner
{
    public static string[] ScanFolder(string folder)
    {
        var subfolders = Directory.GetDirectories(folder);
        return subfolders;
    }
}