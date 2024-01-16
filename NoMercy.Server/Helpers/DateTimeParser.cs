using System.Globalization;

namespace NoMercy.Server.Helpers;

public static class DateTimeParser
{
    public static int _parseYear(string? dateString = null)
    {
        DateTime date;

        if (dateString == null)
        {
            date = DateTime.Now;
        }
        else
        {
            date = DateTime.ParseExact(dateString, "d-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }        
        
        return date.Year;
    }
    
    public static int? ParseYear<T>(this T? self)
    {        
        return _parseYear(self?.ToString());
    }
}