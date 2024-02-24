using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Queue.system;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using LogLevel = NoMercy.Helpers.LogLevel;
using Movie = NoMercy.Database.Models.Movie;
using Translation = NoMercy.Database.Models.Translation;

namespace NoMercy.Server.Logic;

public class MovieLogic(int id, Library library)
{
    private MovieClient MovieClient { get; set; } = new(id);
    private string MediaType { get; set; } = "";
    private string Folder { get; set; } = "";

    public MovieAppends? Movie { get; set; }
    
    private readonly MediaContext _mediaContext = new();

    public async Task Process()
    {
        lock (_mediaContext)
        {
            // var movie = _mediaContext.Movies.FirstOrDefault(e => e.Id == Id);
            // if (movie is not null)
            // {
            //     Logger.MovieDb($@"Movie {movie.Title}: already exists");
            //     return;
            // }
            
            Movie = MovieClient.WithAllAppends().Result;
            if (Movie is null)
            {
                Logger.MovieDb($@"Movie {MovieClient.Id}: not found");
                return;
            }
            
            Folder = FindFolder();
            Logger.MovieDb(Folder);
            MediaType = GetMediaType();
            
            Store().Wait();
            
            StoreAlternativeTitles().Wait();
            StoreTranslations().Wait();
            StoreGenres().Wait();
            StoreKeywords().Wait();
            StoreCompanies().Wait();
            StoreNetworks().Wait();
            StoreVideos().Wait();
            StoreRecommendations().Wait();
            StoreSimilar().Wait();
            StoreContentRatings().Wait();
            StoreWatchProviders().Wait();
        }
        
        await DispatchJobs();
    }

    private async Task StoreAlternativeTitles()
    {
        var alternativeTitles = Movie?.AlternativeTitles.Results.ToList()
            .ConvertAll<AlternativeTitle>(x => new AlternativeTitle(x, Movie.Id)).ToArray() ?? [];
        
        await _mediaContext.AlternativeTitles.UpsertRange(alternativeTitles)
            .On(a => new { a.Title, a.MovieId })
            .WhenMatched((ats, ati) => new AlternativeTitle
            {
                Title = ati.Title,
                Iso31661 = ati.Iso31661,
                MovieId = ati.MovieId,
            })
            .RunAsync();
        
        Logger.MovieDb($@"Movie {Movie?.Title}: AlternativeTitles stored");
    }

    private async Task StoreWatchProviders()
    {
        Logger.MovieDb($@"Movie {Movie?.Title}: WatchProviders stored");
        await Task.CompletedTask;
    }

    private async Task StoreTranslations()
    {
        try
        {
            var translations = Movie?.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, Movie)).ToArray() ?? [];
            
            await _mediaContext.Translations
                .UpsertRange(translations.Where(translation => translation.Title != null || translation.Overview != ""))
                .On(t => new { t.Iso31661, t.Iso6391, t.MovieId })
                .WhenMatched((ts, ti) => new Translation
                {
                    Iso31661 = ti.Iso31661,
                    Iso6391 = ti.Iso6391,
                    Title = ti.Title,
                    EnglishName = ti.EnglishName,
                    Name = ti.Name,
                    Overview = ti.Overview,
                    Homepage = ti.Homepage,
                    Biography = ti.Biography,
                    MovieId = ti.MovieId,
                    SeasonId = ti.SeasonId,
                    EpisodeId = ti.EpisodeId,
                    CollectionId = ti.CollectionId,
                    PersonId = ti.PersonId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
            throw;
        }
    }

    private async Task StoreContentRatings()
    {
        try
        {
            List<CertificationMovie> certifications = [];
        
            foreach (var movieContentRating in Movie?.ReleaseDates?.Results ?? [])
            {
                var certification = _mediaContext.Certifications
                    .FirstOrDefault(c => c.Iso31661 == movieContentRating.Iso31661 && c.Rating == movieContentRating.ReleaseDates[0].Certification);
                if(certification is null) continue;
            
                certifications.Add(new CertificationMovie(certification, Movie));
            }
            
            await _mediaContext.CertificationMovie.UpsertRange(certifications)
                .On(v => new { v.CertificationId, v.MovieId })
                .WhenMatched((ts, ti) => new CertificationMovie
                {
                    CertificationId = ti.CertificationId,
                    MovieId = ti.MovieId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        Logger.MovieDb($@"Movie {Movie?.Title}: ContentRatings stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreSimilar()
    {
        var data = Movie?.Similar?.Results.ToList()
            .ConvertAll<Similar>(x => new Similar(x, Movie)) ?? [];
        
        await _mediaContext.Similar.UpsertRange(data)
            .On(v => new { v.MediaId, v.MovieFromId })
            .WhenMatched((ts, ti) => new Similar
            {
                MovieToId = ti.MovieToId,
                MovieFromId = ti.MovieFromId,
                Overview = ti.Overview,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                MediaId = ti.MediaId,
            })
            .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Similar stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreRecommendations()
    {
        var data = Movie?.Recommendations?.Results.ToList()
            .ConvertAll<Recommendation>(x => new Recommendation(x, Movie)) ?? [];
        
        await _mediaContext.Recommendations.UpsertRange(data)
            .On(v => new { v.MediaId, v.MovieFromId })
            .WhenMatched((ts, ti) => new Recommendation
            {
                MovieToId = ti.MovieToId,
                MovieFromId = ti.MovieFromId,
                Overview = ti.Overview,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                MediaId = ti.MediaId,
            })
            .RunAsync();
        
        Logger.MovieDb($@"Movie {Movie?.Title}: Recommendations stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreVideos()
    {
        try
        {
            var videos = Movie?.Videos.Results.ToList()
                .ConvertAll<Media>(x => new Media(x, Movie, "video")) ?? [];
            
            await _mediaContext.Medias.UpsertRange(videos)
                .On(v => new { v.Src, v.MovieId })
                .WhenMatched((ts, ti) => new Media
                {
                    Src = ti.Src,
                    Iso6391 = ti.Iso6391,
                    Type = ti.Type,
                    MovieId = ti.MovieId,
                    Name = ti.Name,
                    Site = ti.Site,
                    Size = ti.Size,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Logger.MovieDb($@"Movie {Movie?.Title}: Videos stored");
        await Task.CompletedTask;
    }
    
    public static async Task StoreImages(int id)
    {
        MovieClient movieClient = new(id);
        MovieAppends? movie = movieClient.WithAllAppends().Result;
        if (movie is null) return;
        
        List<Image> posters = movie.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image:image, movie:movie, type:"poster")) ?? [];
        
        List<Image> backdrops = movie.Images.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image:image, movie:movie, type:"backdrop")) ?? [];
        
        List<Image> logos = movie.Images.Logos.ToList()
            .ConvertAll<Image>(image => new Image(image:image, movie:movie, type:"logo")) ?? [];
        
        List<Image> images = posters
            .Concat(backdrops)
            .Concat(logos)
            .ToList();
        
        await using MediaContext mediaContext = new();
        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.MovieId })
            .WhenMatched((ts, ti) => new Image
            {
                AspectRatio = ti.AspectRatio,
                FilePath = ti.FilePath,
                Height = ti.Height,
                Iso6391 = ti.Iso6391,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Width = ti.Width,
                Type = ti.Type,
                MovieId = ti.MovieId,
            })
            .RunAsync();
        
        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            ColorPaletteJob colorPaletteJob = new ColorPaletteJob(filePath:image.FilePath, model:"image");
            JobDispatcher.Dispatch(colorPaletteJob, "data");
        }

        Logger.MovieDb($@"Movie {movie.Title}: Images stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreNetworks()
    {
        // var keywords = Movie?.Networks.Results.ToList()
        //     .ConvertAll<Network>(x => new Network(x)).ToArray() ?? [];
        //
        // await _mediaContext.Networks.UpsertRange(keywords)
        //     .On(v => new { v.Id })
        //     .WhenMatched((ts, ti) => new Network
        //     {
        //         Id = ti.Id,
        //         Title = ti.Title,
        //     })
        //     .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Networks stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreCompanies()
    {
        // var companies = Movie?.ProductionCompanies.Results.ToList()
        //     .ConvertAll<ProductionCompany>(x => new ProductionCompany(x)).ToArray() ?? [];
        //
        // await _mediaContext.Companies.UpsertRange(companies)
        //     .On(v => new { v.Id })
        //     .WhenMatched((ts, ti) => new ProductionCompany
        //     {
        //         Id = ti.Id,
        //         Title = ti.Title,
        //     })
        //     .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Companies stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreKeywords()
    {
        var keywords = Movie?.Keywords?.Results.ToList()
            .ConvertAll<Keyword>(x => new Keyword(x)).ToArray() ?? [];
        
        await _mediaContext.Keywords.UpsertRange(keywords)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Keyword
            {
                Id = ti.Id,
                Name = ti.Name,
            })
            .RunAsync();
        
        var keywordMovies = Movie?.Keywords?.Results.ToList()
            .ConvertAll<KeywordMovie>(x => new KeywordMovie(x, Movie)).ToArray() ?? [];

        await _mediaContext.KeywordMovie.UpsertRange(keywordMovies)
            .On(v => new { v.KeywordId, v.MovieId })
            .WhenMatched((ts, ti) => new KeywordMovie()
            {
                KeywordId = ti.KeywordId,
                MovieId = ti.MovieId,
            })
            .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Keywords stored");
        await Task.CompletedTask;
    }
    
    private async Task StoreGenres()
    {
        var genreMovies = Movie?.Genres.ToList()
            .ConvertAll<GenreMovie>(x => new GenreMovie(x, Movie)).ToArray() ?? [];
        
        await _mediaContext.GenreMovie.UpsertRange(genreMovies)
            .On(v => new { v.GenreId, v.MovieId })
            .WhenMatched((ts, ti) => new GenreMovie
            {
                GenreId = ti.GenreId,
                MovieId = ti.MovieId,
            })
            .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Genres stored");
        await Task.CompletedTask;
    }
    
    private static string GetMediaType()
    {
        const string defaultMediaType = "movie";

        return defaultMediaType;
    }

    private string FindFolder()
    {
        if (Movie == null)
        {
            return "";
        }
        return FileNameParsers.CreateBaseFolder(Movie);
    }
    
    private async Task Store()
    {
        if (Movie == null) return;
        
        Movie movieResponse = new Movie(Movie, library.Id, Folder);
        
        await _mediaContext.Movies.Upsert(movieResponse)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Movie
            {
                Id = ti.Id,
                Backdrop = ti.Backdrop,
                Duration = ti.Duration,
                ReleaseDate = ti.ReleaseDate,
                Homepage = ti.Homepage,
                ImdbId = ti.ImdbId,
                OriginalLanguage = ti.OriginalLanguage,
                Overview = ti.Overview,
                Popularity = ti.Popularity,
                Poster = ti.Poster,
                Status = ti.Status,
                Tagline = ti.Tagline,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Trailer = ti.Trailer,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Folder = ti.Folder,
                LibraryId = ti.LibraryId,
                _colorPalette = ti._colorPalette,
                UpdatedAt = ti.UpdatedAt,
            })
            .RunAsync();
        
        await _mediaContext.LibraryMovie.Upsert(new LibraryMovie(library.Id, Movie.Id))
            .On(v => new { v.LibraryId, v.MovieId })
            .WhenMatched((lts, lti) => new LibraryMovie
            {
                LibraryId = lti.LibraryId,
                MovieId = lti.MovieId,
            })
            .RunAsync();
        
        Logger.MovieDb($@"Movie {Movie.Title}: Movie stored");
    }

    private Task DispatchJobs()
    {
        if (Movie is null) return Task.CompletedTask;
        
        PersonJob personJob = new PersonJob(id:Movie.Id, type:"movie");
        JobDispatcher.Dispatch(personJob, "queue", 4);
        
        ColorPaletteJob colorPaletteMovieJob = new ColorPaletteJob(id:Movie.Id, model:"movie");
        JobDispatcher.Dispatch(colorPaletteMovieJob, "data");
        
        ColorPaletteJob colorPaletteSimilarJob = new ColorPaletteJob(id:Movie.Id, model:"similar", type:"movie");
        JobDispatcher.Dispatch(colorPaletteSimilarJob, "data");
        
        ColorPaletteJob colorPaletteRecommendationJob = new ColorPaletteJob(id:Movie.Id, model:"recommendation", type:"movie");
        JobDispatcher.Dispatch(colorPaletteRecommendationJob, "data");
        
        ImagesJob imagesJob = new ImagesJob(id:Movie.Id, type:"movie");
        JobDispatcher.Dispatch(imagesJob, "queue");
        
        if(Movie.BelongsToCollection is null) return Task.CompletedTask;
        CollectionJob collectionJob = new CollectionJob(id:Movie.BelongsToCollection.Id, libraryId:library.Id.ToString());
        JobDispatcher.Dispatch(collectionJob, "queue", 4);
        
        return Task.CompletedTask;
    }
    
    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new();

        var show = await mediaContext.Movies
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if(show is null) return;

        lock (show)
        {
            if (show is { _colorPalette: "" })
            {
                var palette = ImageLogic.GenerateColorPalette(posterPath: show.Poster, backdropPath: show.Backdrop).Result;
                show._colorPalette = palette;
                mediaContext.SaveChanges();
            }
            
        }

    }
    
    public static async Task GetSimilarPalette(int id)
    {
        await using MediaContext mediaContext = new();

        var similars = await mediaContext.Similar
            .Where(e => e.MovieFromId == id)
            .ToListAsync();
        
        if(similars.Count is 0) return;

        lock (similars)
        {
            foreach (var similar in similars)
            {
                if (similar is { _colorPalette: "" })
                {
                    var palette = ImageLogic.GenerateColorPalette(posterPath: similar.Poster, backdropPath: similar.Backdrop).Result;
                    similar._colorPalette = palette;
                    mediaContext.SaveChanges();
                }
            }
        }

    }
    
    public static async Task GetRecommendationPalette(int id)
    {
        await using MediaContext mediaContext = new();

        var recommendations = await mediaContext.Recommendations
            .Where(e => e.MovieFromId == id)
            .ToListAsync();
        
        if(recommendations.Count is 0) return;

        lock (recommendations)
        {
            foreach (var recommendation in recommendations)
            {
                if (recommendation is { _colorPalette: "" })
                {
                    var palette = ImageLogic.GenerateColorPalette(posterPath: recommendation.Poster, backdropPath: recommendation.Backdrop).Result;
                    recommendation._colorPalette = palette;
                    mediaContext.SaveChanges();
                }
            }
        }
    }
    
    public void Dispose()
    {
        Movie = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
    
}