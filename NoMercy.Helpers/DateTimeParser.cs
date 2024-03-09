using System.Globalization;

namespace NoMercy.Helpers;

public static class DateTimeParser
{
    private static int _parseYear(DateTime? dateString = null)
    {
        DateTime date = string.IsNullOrEmpty(dateString.ToString())
            ? DateTime.Now 
            : DateTime.Parse(dateString.ToString() ?? string.Empty, CultureInfo.InvariantCulture);        
        
        return date.Year;
    }

    public static int ParseYear(this DateTime? self)
    {
        try
        {
            return string.IsNullOrEmpty(self.ToString())
                ? 0 
                : _parseYear(self);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static string ToHms(this int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
    }
    
}