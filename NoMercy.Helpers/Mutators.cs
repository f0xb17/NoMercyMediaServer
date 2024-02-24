
namespace NoMercy.Helpers;

public static class Mutators
{
    private static Random rand = new();  

    // public static void Shuffle<T>(this IList<T> list)  
    // {  
    //     int n = list.Count;  
    //     while (n > 1) {  
    //         n--;  
    //         int k = rand.Next(n + 1);  
    //         (list[k], list[n]) = (list[n], list[k]);
    //     }  
    // }
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
    {
        Random rnd = new Random();
        return source.OrderBy(_ => rnd.Next());
    }
}
