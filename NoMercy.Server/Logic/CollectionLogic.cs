using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using Collection = NoMercy.Database.Models.Collection;
using Movie = NoMercy.Database.Models.Movie;

namespace NoMercy.Server.Logic;

public class CollectionLogic(int id, Library library) : IDisposable, IAsyncDisposable
{
    private TmdbCollectionClient TmdbCollectionClient { get; set; } = new(id);
    private MediaContext MediaContext { get; set; } = new();

    public TmdbCollectionAppends? Collection { get; set; }

    public async Task Process()
    {
        // Collection? special = _mediaContext.Collections.FirstOrDefault(e => e.Id == id);
        // if (special is not null)
        // {
        //     Logger.MovieDb($@"Collection {special.Title}: already exists");
        //     return;
        // }

        Collection = await TmdbCollectionClient.WithAllAppends();
        if (Collection is null)
        {
            Logger.MovieDb($@"Collection {TmdbCollectionClient.Id}: not found");
            return;
        }

        await Store();

        await StoreTranslations();

        await DispatchJobs();
    }

    private async Task Store()
    {
        if (Collection == null) return;

        Collection collections = new(Collection, library.Id)
        {
            _colorPalette = MovieDbImage
                .MultiColorPalette(new[]
                {
                    new BaseImage.MultiStringType("poster", Collection.PosterPath),
                    new BaseImage.MultiStringType("backdrop", Collection.BackdropPath)
                }).Result
        };

        await MediaContext.Collections.Upsert(collections)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Collection
            {
                Id = ti.Id,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                Title = ti.Title,
                Overview = ti.Overview,
                Parts = ti.Parts,
                LibraryId = ti.LibraryId,
                TitleSort = ti.TitleSort,
                _colorPalette = ti._colorPalette
            })
            .RunAsync();

        Logger.MovieDb($@"Collection {Collection?.Name} stored");

        Movie[] movies = Collection?.Parts.ToList()
            .ConvertAll<Movie>(x => new Movie(x, library.Id)).ToArray() ?? [];

        await MediaContext.Movies.UpsertRange(movies)
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
                UpdatedAt = ti.UpdatedAt,
                LibraryId = ti.LibraryId
            })
            .RunAsync();

        // foreach (Movie movie in movies)
        // {
        //     ColorPaletteJob colorPaletteJob = new(id: movie.Id, model: "movie");
        //     JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        // }

        Logger.MovieDb($@"Collection {Collection?.Name} movies stored");

        CollectionMovie[] collectionMovies = Collection?.Parts.ToList()
            .ConvertAll<CollectionMovie>(x => new CollectionMovie(x, collections.Id)).ToArray() ?? [];

        await MediaContext.CollectionMovie.UpsertRange(collectionMovies)
            .On(v => new { v.CollectionId, v.MovieId })
            .WhenMatched((ts, ti) => new CollectionMovie
            {
                CollectionId = ti.CollectionId,
                MovieId = ti.MovieId
            })
            .RunAsync();

        await Task.CompletedTask;
    }

    private async Task DispatchJobs()
    {
        if (Collection is null) return;

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
        {
            QueryKey = ["special", Collection.Id]
        });

        foreach (var movie in Collection.Parts)
        {
            TmdbMovieClient tmdbMovieClient = new(movie.Id);
            var movieAppends = await tmdbMovieClient.WithAllAppends();
            if (movieAppends is null) continue;

            Logger.MovieDb($@"Collection {Collection.Name}: Dispatching movie {movieAppends.Title}");

            PersonJob personJob = new(movieAppends.Id, "movie");
            JobDispatcher.Dispatch(personJob, "queue", 3);

            MovieImagesJob movieImagesJob = new(movieAppends.Id);
            JobDispatcher.Dispatch(movieImagesJob, "data", 2);
        }

        CollectionImagesJob imagesJob = new(id);
        JobDispatcher.Dispatch(imagesJob, "data", 2);
    }

    private async Task StoreTranslations()
    {
        Translation[] translations = Collection?.Translations.Translations.ToList()
            .ConvertAll<Translation>(x => new Translation(x, Collection)).ToArray() ?? [];

        await using MediaContext mediaContext = new();
        await mediaContext.Translations
            .UpsertRange(translations.Where(translation => translation.Title != null || translation.Overview != ""))
            .On(t => new { t.Iso31661, t.Iso6391, t.CollectionId })
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
                SeasonId = ti.SeasonId,
                EpisodeId = ti.EpisodeId,
                CollectionId = ti.CollectionId,
                PersonId = ti.PersonId
            })
            .RunAsync();
    }

    public static async Task StoreImages(int id)
    {
        TmdbCollectionClient tmdbCollectionClient = new(id);
        var collection = await tmdbCollectionClient.WithAllAppends();
        if (collection is null) return;

        List<Image> posters = collection.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image, collection, "poster"));

        List<Image> backdrops = collection.Images.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image, collection, "backdrop"));

        // TODO: Future for when they add logos
        // List<Image> logos = special?.Images?.Logos.ToList()
        //     .ConvertAll<Image>(image => new Image(image:image, special:special, type:"logo"));

        List<Image> images = posters
            .Concat(backdrops)
            // .Concat(logos)
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
                CollectionId = ti.CollectionId
            })
            .RunAsync();

        // ColorPaletteJob colorPaletteJob = new(id: id, model: "special");
        // JobDispatcher.Dispatch(colorPaletteJob, "image", 2);

        Logger.MovieDb($@"Collection {collection.Name}: Images stored");
        await Task.CompletedTask;
    }

    public static async Task Palette(int id)
    {
        Logger.Queue($"Fetching color palette for Special {id}");
        await using MediaContext mediaContext = new();

        var collection = await mediaContext.Collections
            .Where(e => e._colorPalette == "")
            .FirstOrDefaultAsync(e => e.Id == id);

        if (collection is { _colorPalette: "" })
        {
            collection._colorPalette = await MovieDbImage
                .MultiColorPalette(new[]
                {
                    new BaseImage.MultiStringType("poster", collection.Poster),
                    new BaseImage.MultiStringType("backdrop", collection.Backdrop)
                });

            await mediaContext.SaveChangesAsync();
        }

        Image[] images = await mediaContext.Images
            .Where(e => e.CollectionId == id)
            .Where(e => e._colorPalette == "")
            .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
                        e.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            .ToArrayAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;

            Logger.Queue($"Fetching color palette for Collection Image {image.FilePath}");

            ColorPaletteJob colorPaletteJob = new(image.FilePath, "image", image.Iso6391);
            JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        }
    }

    public void Dispose()
    {
        Collection = null;
        MediaContext.Dispose();
        TmdbCollectionClient.Dispose();
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public async ValueTask DisposeAsync()
    {
        Collection = null;
        await MediaContext.DisposeAsync();
        await TmdbCollectionClient.DisposeAsync();
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }
}