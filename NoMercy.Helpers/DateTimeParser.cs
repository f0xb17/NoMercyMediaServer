using System.Globalization;

namespace NoMercy.Helpers;

public static class DateTimeParser
{
    private static int _parseYear(DateTime? dateString = null)
    {
        DateTime date = dateString == null 
            ? DateTime.Now 
            : DateTime.ParseExact(dateString.ToString() ?? string.Empty, "d-M-yyyy HH:mm:ss", CultureInfo.InvariantCulture);        
        
        return date.Year;
    }

    public static int ParseYear(this DateTime? self)
    {
        return self == null 
            ? 0 
            : _parseYear(self);
    }
    
}