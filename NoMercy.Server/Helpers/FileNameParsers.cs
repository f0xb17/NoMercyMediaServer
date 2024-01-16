namespace NoMercy.Helpers;

public class FileNameParsers
{
    public static string CreateBaseFolder(TvShow folder)
    {
        return folder.Replace(" ", ".");
    }
}