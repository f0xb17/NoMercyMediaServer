using System.Globalization;
using System.Text;

namespace NoMercy.Helpers;

public static class Str
{
    public static double MatchPercentage(string strA, string strB)
    {
        int result = 0;
        for (int i = strA.Length - 1; i >= 0; i--)
        {
            if (i >= strB.Length || strA[i] == strB[i])
            {
                // Do nothing
            }
            else if (char.ToLower(strA[i]) == char.ToLower(strB[i]))
            {
                result += 1;
            }
            else
            {
                result += 4;
            }
        }
        return 100 - (result + 4 * Math.Abs(strA.Length - strB.Length)) / (2.0 * (strA.Length + strB.Length)) * 100;
    }

    public static List<T> SortByMatchPercentage<T>(IEnumerable<T> array, Func<T, string> keySelector, string match)
    {
        return array.OrderBy(item => MatchPercentage(match, keySelector(item))).ToList();
    }
    
    public static string RemoveAccents(this string s)
    {
        Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");

        return destEncoding.GetString(
            Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
    }

    public static string RemoveDiacritics(this string text)
    {
        string formD = text.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char ch in formD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}