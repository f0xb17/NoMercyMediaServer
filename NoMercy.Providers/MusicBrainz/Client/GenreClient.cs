// ReSharper disable All

using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class GenreClient : BaseClient
{
    public GenreClient() : base()
    {
    }

    public async Task<List<Genre>> All(int page = 1)
    {
        List<Genre> genres = [];
        
        var data = await Get<AllGenres>("genre/all", new Dictionary<string, string>
        {
            ["limit"] = 100.ToString(),
            ["offset"] = ((page - 1) * 100).ToString(),
            ["fmt"] = "json"
        });
        
        if (data is null) return genres;
        
        genres.AddRange(data.Genres);
        
        for (int i = 0; i < data.GenreCount / data.Genres.Length; i++)
        {
            var data2 = await Get<AllGenres>("genre/all", new Dictionary<string, string>
            {
                ["limit"] = data.Genres.Length.ToString(),
                ["offset"] = (i * data.Genres.Length).ToString()
            });
            
            if (data2 is null) continue;
            
            genres.AddRange(data2.Genres);
        }
        
        return genres;
    }
    
}

