using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;
using Movie = NoMercy.Database.Models.Movie;
using Translation = NoMercy.Database.Models.Translation;

namespace NoMercy.Server.Logic;

public class MovieLogic(int id, Library library) : IDisposable, IAsyncDisposable
{
    private TmdbMovieClient TmdbMovieClient { get; } = new(id);
    private string MediaType { get; set; } = "";
    private string Folder { get; set; } = "";

    public TmdbMovieAppends? Movie { get; private set; }

    private readonly MediaContext _mediaContext = new();

    public async Task Process()
    {
        // Movie? movie = _mediaContext.Movies.FirstOrDefault(e => e.Id == Id);
        // if (movie is not null)
        // {
        //     Logger.MovieDb($@"Movie {movie.Title}: already exists");
        //     return;
        // }

        Movie = await TmdbMovieClient.WithAllAppends();
        if (Movie is null)
        {
            Logger.MovieDb($@"Movie {TmdbMovieClient.Id}: not found");
            return;
        }

        Folder = FindFolder();
        MediaType = MakeMediaType();

        await Store();

        await StoreAlternativeTitles();
        await StoreTranslations();
        await StoreGenres();
        await StoreKeywords();
        await StoreCompanies();
        await StoreNetworks();
        await StoreVideos();
        await StoreRecommendations();
        await StoreSimilar();
        await StoreContentRatings();
        await StoreWatchProviders();

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
                MovieId = ati.MovieId
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
                .UpsertRange(translations.Where(translation => translation.Title != "" || translation.Overview != ""))
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
                    PersonId = ti.PersonId
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

            foreach (var movieContentRating in Movie?.ReleaseDates.Results ?? [])
            {
                var certification = _mediaContext.Certifications
                    .FirstOrDefault(c =>
                        c.Iso31661 == movieContentRating.Iso31661 &&
                        c.Rating == movieContentRating.ReleaseDates[0].Certification);
                if (certification is null) continue;

                certifications.Add(new CertificationMovie(certification, Movie));
            }

            await _mediaContext.CertificationMovie.UpsertRange(certifications)
                .On(v => new { v.CertificationId, v.MovieId })
                .WhenMatched((ts, ti) => new CertificationMovie
                {
                    CertificationId = ti.CertificationId,
                    MovieId = ti.MovieId
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
        var data = Movie?.Similar.Results.ToList()
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
                MediaId = ti.MediaId
            })
            .RunAsync();

        ColorPaletteJob similarColorPaletteJob = new(id, model: "similar", type: "movie");
        JobDispatcher.Dispatch(similarColorPaletteJob, "image", 2);

        Logger.MovieDb($@"Movie {Movie?.Title}: Similar stored");
        await Task.CompletedTask;
    }

    private async Task StoreRecommendations()
    {
        var data = Movie?.Recommendations.Results.ToList()
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
                MediaId = ti.MediaId
            })
            .RunAsync();

        ColorPaletteJob recommendationColorPaletteJob = new(id, model: "recommendation", type: "movie");
        JobDispatcher.Dispatch(recommendationColorPaletteJob, "image", 2);

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
                    Size = ti.Size
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
        TmdbMovieClient tmdbMovieClient = new(id);
        var movie = await tmdbMovieClient.WithAllAppends();
        if (movie is null) return;

        var posters = movie.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image, movie, "poster"));

        List<Image> backdrops = movie.Images.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image, movie, "backdrop"));

        List<Image> logos = movie.Images.Logos.ToList()
            .ConvertAll<Image>(image => new Image(image, movie, "logo"));

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
                Site = ti.Site,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Width = ti.Width,
                Type = ti.Type,
                MovieId = ti.MovieId
            })
            .RunAsync();

        ColorPaletteJob movieColorPaletteJob = new(id, "movie");
        JobDispatcher.Dispatch(movieColorPaletteJob, "image", 2);

        Logger.MovieDb($@"Movie {movie.Title}: Images stored");
        await Task.CompletedTask;
    }

    private async Task StoreNetworks()
    {
        // List<Keyword> keywords = Movie?.Networks.Results.ToList()
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
        // List<Company> companies = Movie?.ProductionCompanies.Results.ToList()
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
        var keywords = Movie?.Keywords.Results.ToList()
            .ConvertAll<Keyword>(x => new Keyword(x)).ToArray() ?? [];

        await _mediaContext.Keywords.UpsertRange(keywords)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Keyword
            {
                Id = ti.Id,
                Name = ti.Name
            })
            .RunAsync();

        KeywordMovie[] keywordMovies = Movie?.Keywords.Results.ToList()
            .ConvertAll<KeywordMovie>(x => new KeywordMovie(x, Movie)).ToArray() ?? [];

        await _mediaContext.KeywordMovie.UpsertRange(keywordMovies)
            .On(v => new { v.KeywordId, v.MovieId })
            .WhenMatched((ts, ti) => new KeywordMovie()
            {
                KeywordId = ti.KeywordId,
                MovieId = ti.MovieId
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
                MovieId = ti.MovieId
            })
            .RunAsync();

        Logger.MovieDb($@"Movie {Movie?.Title}: Genres stored");
        await Task.CompletedTask;
    }

    private static string MakeMediaType()
    {
        const string defaultMediaType = "movie";

        return defaultMediaType;
    }

    private string FindFolder()
    {
        if (Movie == null) return "";

        return FileNameParsers.CreateBaseFolder(Movie);
    }

    private async Task Store()
    {
        if (Movie == null) return;

        Movie movieResponse = new(Movie, library.Id, Folder)
        {
            _colorPalette = await MovieDbImage
                .MultiColorPalette(new[]
                {
                new BaseImage.MultiStringType("poster", Movie.PosterPath),
                new BaseImage.MultiStringType("backdrop", Movie.BackdropPath)
            })
        };

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
                UpdatedAt = ti.UpdatedAt
            })
            .RunAsync();

        await _mediaContext.LibraryMovie.Upsert(new LibraryMovie(library.Id, Movie.Id))
            .On(v => new { v.LibraryId, v.MovieId })
            .WhenMatched((lts, lti) => new LibraryMovie
            {
                LibraryId = lti.LibraryId,
                MovieId = lti.MovieId
            })
            .RunAsync();

        Logger.MovieDb($@"Movie {Movie.Title}: Movie stored");
    }

    private Task DispatchJobs()
    {
        if (Movie is null) return Task.CompletedTask;

        FindMediaFilesJob findMediaFilesJob = new(id, library);
        JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);

        PersonJob personJob = new(id, "movie");
        JobDispatcher.Dispatch(personJob, "queue", 3);

        MovieImagesJob imagesJob = new(id);
        JobDispatcher.Dispatch(imagesJob, "data", 2);

        if (Movie.BelongsToCollection is null) return Task.CompletedTask;

        AddCollectionJob addCollectionJob = new(Movie.BelongsToCollection.Id, library);
        JobDispatcher.Dispatch(addCollectionJob, "queue", 4);

        return Task.CompletedTask;
    }

    public static async Task Palette(int id, bool download = true)
    {
        Logger.Queue($"Fetching color palette for Movie {id}");

        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .Where(e => e._colorPalette == "")
            .FirstOrDefaultAsync(e => e.Id == id);

        if (movie is { _colorPalette: "" })
        {
            movie._colorPalette = await MovieDbImage
                .MultiColorPalette(new[]
                {
                    new BaseImage.MultiStringType("poster", movie.Poster),
                    new BaseImage.MultiStringType("backdrop", movie.Backdrop)
                });

            await mediaContext.SaveChangesAsync();
        }

        var images = await mediaContext.Images
            .Where(e => e.MovieId == id)
            .Where(e => e._colorPalette == "")
            .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
                        e.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            .ToArrayAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;

            image._colorPalette = await MovieDbImage.ColorPalette("image", image.FilePath);
        }
        
        await mediaContext.SaveChangesAsync();
    }

    public static async Task SimilarPalette(int id)
    {
        await using MediaContext mediaContext = new();
        var similarList = await mediaContext.Similar
            .Where(e => e.MovieFromId == id && e._colorPalette == "")
            .ToListAsync();

        if (similarList.Count is 0) return;

        foreach (var similar in similarList)
        {
            if (similar is not { _colorPalette: "" }) continue;

            similar._colorPalette = await MovieDbImage
                .MultiColorPalette(new[]
                {
                    new BaseImage.MultiStringType("poster", similar.Poster),
                    new BaseImage.MultiStringType("backdrop", similar.Backdrop)
                });
        }

        await mediaContext.SaveChangesAsync();
    }

    public static async Task RecommendationPalette(int id)
    {
        await using MediaContext mediaContext = new();

        var recommendations = await mediaContext.Recommendations
            .Where(e => e.MovieFromId == id && e._colorPalette == "")
            .ToListAsync();

        if (recommendations.Count is 0) return;

        foreach (var recommendation in recommendations)
        {
            if (recommendation is not { _colorPalette: "" }) continue;

            recommendation._colorPalette = await MovieDbImage
                .MultiColorPalette(new[]
                {
                    new BaseImage.MultiStringType("poster", recommendation.Poster),
                    new BaseImage.MultiStringType("backdrop", recommendation.Backdrop)
                });
        }

        await mediaContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Movie = null;
        _mediaContext.Dispose();
        TmdbMovieClient.Dispose();
        GC.Collect();
        GC.WaitForFullGCComplete();
    }

    public async ValueTask DisposeAsync()
    {
        Movie = null;
        await _mediaContext.DisposeAsync();
        await TmdbMovieClient.DisposeAsync();
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}