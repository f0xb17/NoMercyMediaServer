using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.Collections;

public class CollectionRepository(MediaContext context) : ICollectionRepository
{
    public Task Store(Collection collection)
    {
        return context.Collections.Upsert(collection)
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
    }

    public Task LinkToLibrary(Library library, Collection collection)
    {
        return context.CollectionLibrary.Upsert(new CollectionLibrary
        {
            LibraryId = library.Id,
            CollectionId = collection.Id
        })
            .On(v => new { v.LibraryId, v.CollectionId })
            .WhenMatched((ts, ti) => new CollectionLibrary
            {
                LibraryId = ti.LibraryId,
                CollectionId = ti.CollectionId
            })
            .RunAsync();
    }

    public Task StoreAlternativeTitles(IEnumerable<AlternativeTitle> alternativeTitles)
    {
        // return context.AlternativeTitles.UpsertRange(alternativeTitles)
        //     .On(v => new { v.Iso31661, v.CollectionId })
        //     .WhenMatched((ts, ti) => new AlternativeTitle
        //     {
        //         Iso31661 = ti.Iso31661,
        //         Title = ti.Title,
        //         CollectionId = ti.CollectionId
        //     })
        //     .RunAsync();
        
        return Task.CompletedTask;
    }

    public Task StoreTranslations(IEnumerable<Translation> translations)
    {
        return context.Translations
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

    public Task StoreImages(IEnumerable<Image> images)
    {
        return context.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.CollectionId })
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
    }
}