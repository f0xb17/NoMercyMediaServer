using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Queue.system;
using NoMercy.Server.app.Jobs;
using Collection = NoMercy.Database.Models.Collection;
using LogLevel = NoMercy.Helpers.LogLevel;
using Movie = NoMercy.Database.Models.Movie;

namespace NoMercy.Server.Logic;

public class CollectionLogic(int id, Library library)
{
    private CollectionClient CollectionClient { get; set; } = new(id);

    public CollectionAppends? Collection { get; set; }
    
    private readonly MediaContext _mediaContext = new();

    public async Task Process()
    {
        lock (_mediaContext)
        {
            // var collection = _mediaContext.Collections.FirstOrDefault(e => e.Id == id);
            // if (collection is not null)
            // {
            //     Logger.MovieDb($@"Collection {collection.Title}: already exists");
            //     return;
            // }

            Collection = CollectionClient.WithAllAppends().Result;
            if (Collection is null)
            {
                Logger.MovieDb($@"Collection {CollectionClient.Id}: not found");
                return;
            }

            Store().Wait();
            
            StoreTranslations().Wait();
        }
        
        await DispatchJobs();
    }

    private async Task Store()
    {
        if (Collection == null) return;
        
        var collections = new Collection(Collection, library.Id);

        await _mediaContext.Collections.Upsert(collections)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Collection
            {
                Id = ti.Id,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                Title = ti.Title,
                Overview = ti.Overview,
                _colorPalette = ti._colorPalette,
                Parts = ti.Parts,
                LibraryId = ti.LibraryId,
                TitleSort = ti.TitleSort,
            })
            .RunAsync();
        
        Logger.MovieDb($@"Collection {Collection?.Name} stored");
        
        var movies = Collection?.Parts.ToList()
            .ConvertAll<Movie>(x => new Movie(x, library.Id)).ToArray() ?? [];
        
        await _mediaContext.Movies.UpsertRange(movies)
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
                _colorPalette = ti._colorPalette,
                UpdatedAt = ti.UpdatedAt,
                LibraryId = ti.LibraryId,
            })
            .RunAsync();
        
        foreach (var movie in movies)
        {
            ColorPaletteJob colorPaletteMovieJob = new ColorPaletteJob(id:movie.Id, model:"movie");
            JobDispatcher.Dispatch(colorPaletteMovieJob, "data");
        }
        
        Logger.MovieDb($@"Collection {Collection?.Name} movies stored");
        
        var collectionMovies = Collection?.Parts.ToList()
            .ConvertAll<CollectionMovie>(x => new CollectionMovie(x, collections.Id)).ToArray() ?? [];
        
        await _mediaContext.CollectionMovie.UpsertRange(collectionMovies)
            .On(v => new { v.CollectionId, v.MovieId })
            .WhenMatched((ts, ti) => new CollectionMovie
            {
                CollectionId = ti.CollectionId,
                MovieId = ti.MovieId,
            })
            .RunAsync();

        await Task.CompletedTask;

    }
    
    private Task DispatchJobs()
    {
        if (Collection is null) return Task.CompletedTask;

        try
        {
            foreach (var movie in Collection.Parts)
            {
                try
                {
                    MovieClient movieClient = new(movie.Id);
                    MovieAppends? movieAppends = movieClient.WithAllAppends().Result;
                    Logger.MovieDb($@"Collection {Collection.Name}: Dispatching movie {movieAppends.Title}");
                    
                    PersonJob personJob = new PersonJob(id:movieAppends.Id, type:"movie");
                    JobDispatcher.Dispatch(personJob, "queue", 4);
        
                    ImagesJob movieImagesJob = new ImagesJob(id:movieAppends.Id, type:"movie");
                    JobDispatcher.Dispatch(movieImagesJob, "queue");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
            ColorPaletteJob colorPaletteCollectionJob = new ColorPaletteJob(id:Collection.Id, model:"collection");
            JobDispatcher.Dispatch(colorPaletteCollectionJob, "data");
        
            ImagesJob imagesJob = new ImagesJob(id:Collection.Id, type:"collection");
            JobDispatcher.Dispatch(imagesJob, "queue");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return Task.CompletedTask;
    }
    
    private async Task StoreTranslations()
    {
        try
        {
            var translations = Collection?.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, Collection)).ToArray() ?? [];
            
            await _mediaContext.Translations
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
    
    public static async Task StoreImages(int id)
    {
        CollectionClient collectionClient = new(id);
        CollectionAppends? collection = collectionClient.WithAllAppends().Result;
        
        var posters = collection?.Images?.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image:image, collection:collection, type:"poster")) ?? [];
        
        var backdrops = collection?.Images?.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image:image, collection:collection, type:"backdrop")) ?? [];
        
        var logos = collection?.Images?.Logos.ToList()
            .ConvertAll<Image>(image => new Image(image:image, collection:collection, type:"logo")) ?? [];
        
        var images = posters
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
                CollectionId = ti.CollectionId,
            })
            .RunAsync();
        
        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            ColorPaletteJob colorPaletteJob = new ColorPaletteJob(filePath:image.FilePath, model:"image");
            JobDispatcher.Dispatch(colorPaletteJob, "data");
        }

        Logger.MovieDb($@"Collection {collection?.Name}: Images stored");
        await Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        Logger.Queue($"Fetching color palette for collection {id}");
        await using MediaContext mediaContext = new();

        var collection = await mediaContext.Collections
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if(collection is null) return;

        lock (collection)
        {
            if (collection is { _colorPalette: "", Poster: not null, Backdrop: not null })
            {
                var palette = ImageLogic.GenerateColorPalette(posterPath: collection.Poster, backdropPath: collection.Backdrop).Result;
                collection._colorPalette = palette;
                mediaContext.SaveChanges();
            }
        }
    }

    public void Dispose()
    {
        Collection = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}