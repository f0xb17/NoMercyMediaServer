using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using Collection = NoMercy.Database.Models.Collection;
using LogLevel = NoMercy.Helpers.LogLevel;
using Movie = NoMercy.Database.Models.Movie;

namespace NoMercy.Server.Logic;

public class CollectionLogic(int id, Library library)
{
    private CollectionClient CollectionClient { get; set; } = new(id);
    private MediaContext MediaContext { get; set; } = new();

    public CollectionAppends? Collection { get; set; }
    
    public async Task Process()
    {
        // Collection? special = _mediaContext.Collections.FirstOrDefault(e => e.Id == id);
        // if (special is not null)
        // {
        //     Logger.MovieDb($@"Collection {special.Title}: already exists");
        //     return;
        // }

        Collection = await CollectionClient.WithAllAppends();
        if (Collection is null)
        {
            Logger.MovieDb($@"Collection {CollectionClient.Id}: not found");
            return;
        }

        await Store();
        
        await StoreTranslations();
        
        await DispatchJobs();
    }

    private async Task Store()
    {
        if (Collection == null) return;
        
        Collection collections = new Collection(Collection, library.Id);

        await MediaContext.Collections.Upsert(collections)
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
                _colorPalette = ti._colorPalette,
                UpdatedAt = ti.UpdatedAt,
                LibraryId = ti.LibraryId,
            })
            .RunAsync();
        
        foreach (Movie movie in movies)
        {
            try {
                await new ColorPaletteJob(id:movie.Id, model:"movie").Handle();
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }
        
        Logger.MovieDb($@"Collection {Collection?.Name} movies stored");
        
        CollectionMovie[] collectionMovies = Collection?.Parts.ToList()
            .ConvertAll<CollectionMovie>(x => new CollectionMovie(x, collections.Id)).ToArray() ?? [];
        
        await MediaContext.CollectionMovie.UpsertRange(collectionMovies)
            .On(v => new { v.CollectionId, v.MovieId })
            .WhenMatched((ts, ti) => new CollectionMovie
            {
                CollectionId = ti.CollectionId,
                MovieId = ti.MovieId,
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

        foreach (Providers.TMDB.Models.Movies.Movie movie in Collection.Parts)
        {
            MovieClient movieClient = new(movie.Id);
            MovieAppends? movieAppends = await movieClient.WithAllAppends();
            if (movieAppends is null) continue;

            Logger.MovieDb($@"Collection {Collection.Name}: Dispatching movie {movieAppends.Title}");

            try
            {
                PersonJob personJob = new PersonJob(id: movieAppends.Id, type: "movie");
                JobDispatcher.Dispatch(personJob, "queue", 3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                MovieImagesJob movieImagesJob = new MovieImagesJob(id: movieAppends.Id);
                JobDispatcher.Dispatch(movieImagesJob, "data", 2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        try
        {
            await new ColorPaletteJob(id: Collection.Id, model: "special").Handle();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            CollectionImagesJob imagesJob = new CollectionImagesJob(id: Collection.Id);
            JobDispatcher.Dispatch(imagesJob, "data", 2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task StoreTranslations()
    {
        Translation[] translations = Collection?.Translations.Translations.ToList()
            .ConvertAll<Translation>(x => new Translation(x, Collection)).ToArray() ?? [];
            
            await using MediaContext mediaContext = new MediaContext();
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
                    PersonId = ti.PersonId,
                })
                .RunAsync();
    }
    
    public static async Task StoreImages(int id)
    {
        CollectionClient collectionClient = new(id);
        CollectionAppends? collection = await collectionClient.WithAllAppends();
        if (collection is null) return;
        
        List<Image> posters = collection.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image:image, collection:collection, type:"poster"));
        
        List<Image> backdrops = collection.Images.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image:image, collection:collection, type:"backdrop"));
        
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
                CollectionId = ti.CollectionId,
            })
            .RunAsync();
        
        foreach (Image image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            
            try
            {
                await new ColorPaletteJob(filePath:image.FilePath, model:"image", image.Iso6391).Handle();
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }

        Logger.MovieDb($@"Collection {collection.Name}: Images stored");
        await Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        Logger.Queue($"Fetching color palette for special {id}");
        await using MediaContext mediaContext = new();

        Collection? collection = await mediaContext.Collections
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (collection is { _colorPalette: "", Poster: not null, Backdrop: not null })
        {
            string palette = await ImageLogic.GenerateColorPalette(posterPath: collection.Poster, backdropPath: collection.Backdrop);
            collection._colorPalette = palette;
            
            await mediaContext.SaveChangesAsync();
        }
    }

    public void Dispose()
    {
        Collection = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}