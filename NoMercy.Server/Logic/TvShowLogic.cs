using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.Other;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;
using Translation = NoMercy.Database.Models.Translation;

namespace NoMercy.Server.Logic;

public class TvShowLogic(int id, Library library)
{
    private TvClient TvClient { get; } = new(id);
    private string MediaType { get; set; } = "";
    private string Folder { get; set; } = "";

    public TvShowAppends? Show { get; set; }

    private readonly MediaContext _mediaContext = new();

    public async Task Process()
    {
        // Tv? show = _mediaContext.Tvs.FirstOrDefault(e => e.Id == id);
        // if (show is not null)
        // {
        //     Logger.MovieDb($@"TvShow {show.Title}: already exists");
        //     return;
        // }

        Show = await TvClient.WithAllAppends();
        if (Show is null)
        {
            Logger.MovieDb($@"TvShow {TvClient.Id}: not found");
            return;
        }

        Logger.MovieDb($@"TvShow {Show.Name}: found");

        Folder = FindFolder();
        MediaType = GetMediaType();
        
        Logger.MovieDb($@"TvShow {Show.Name}: Folder: {Folder} MediaType: {MediaType}");

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

        SeasonLogic seasonLogic = new SeasonLogic(Show);
        await seasonLogic.FetchSeasons();
        seasonLogic.Dispose();

        await DispatchJobs();
    }

    private async Task StoreAlternativeTitles()
    {
        AlternativeTitle[] alternativeTitles = Show?.AlternativeTitles.Results.ToList()
            .ConvertAll<AlternativeTitle>(x => new AlternativeTitle(x, Show.Id)).ToArray() ?? [];

        await _mediaContext.AlternativeTitles.UpsertRange(alternativeTitles)
            .On(a => new { a.Title, a.TvId })
            .WhenMatched((ats, ati) => new AlternativeTitle
            {
                Title = ati.Title,
                Iso31661 = ati.Iso31661,
                TvId = ati.TvId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: AlternativeTitles stored");
    }

    private async Task StoreAggregateCredits()
    {
        Logger.MovieDb($@"TvShow {Show?.Name}: AggregateCredits stored");
        await Task.CompletedTask;
    }

    private async Task StoreWatchProviders()
    {
        Logger.MovieDb($@"TvShow {Show?.Name}: WatchProviders stored");
        await Task.CompletedTask;
    }

    private async Task StoreTranslations()
    {
        try
        {
            Translation[] translations = Show?.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, Show)).ToArray() ?? [];

            await _mediaContext.Translations
                .UpsertRange(translations.Where(translation => translation.Title != null || translation.Overview != ""))
                .On(t => new { t.Iso31661, t.Iso6391, t.TvId })
                .WhenMatched((ts, ti) => new Translation
                {
                    Iso31661 = ti.Iso31661,
                    Iso6391 = ti.Iso6391,
                    Name = ti.Name,
                    EnglishName = ti.EnglishName,
                    Title = ti.Title,
                    Overview = ti.Overview,
                    Homepage = ti.Homepage,
                    Biography = ti.Biography,
                    TvId = ti.TvId,
                    SeasonId = ti.SeasonId,
                    EpisodeId = ti.EpisodeId,
                    MovieId = ti.MovieId,
                    CollectionId = ti.CollectionId,
                    PersonId = ti.PersonId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreContentRatings()
    {
        List<CertificationTv> certifications = [];

        foreach (TvContentRating tvContentRating in Show?.ContentRatings.Results ?? [])
        {
            Certification? certification = _mediaContext.Certifications
                .FirstOrDefault(c => c.Iso31661 == tvContentRating.Iso31661 && c.Rating == tvContentRating.Rating);
            if (certification is null) continue;

            certifications.Add(new CertificationTv(certification, Show));
        }

        await _mediaContext.CertificationTv.UpsertRange(certifications)
            .On(v => new { v.CertificationId, v.TvId })
            .WhenMatched((ts, ti) => new CertificationTv
            {
                CertificationId = ti.CertificationId,
                TvId = ti.TvId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: ContentRatings stored");
        await Task.CompletedTask;
    }

    private async Task StoreSimilar()
    {
        List<Similar> data = Show?.Similar.Results.ToList()
            .ConvertAll<Similar>(x => new Similar(x, Show)) ?? [];

        await _mediaContext.Similar.UpsertRange(data)
            .On(v => new { v.MediaId, v.TvFromId })
            .WhenMatched((ts, ti) => new Similar
            {
                TvToId = ti.TvToId,
                TvFromId = ti.TvFromId,
                Overview = ti.Overview,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                MediaId = ti.MediaId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Similar stored");
        await Task.CompletedTask;
    }

    private async Task StoreRecommendations()
    {
        List<Recommendation> data = Show?.Recommendations.Results.ToList()
            .ConvertAll<Recommendation>(x => new Recommendation(x, Show)) ?? [];

        await _mediaContext.Recommendations.UpsertRange(data)
            .On(v => new { v.MediaId, v.TvFromId })
            .WhenMatched((ts, ti) => new Recommendation
            {
                TvToId = ti.TvToId,
                TvFromId = ti.TvFromId,
                Overview = ti.Overview,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Backdrop = ti.Backdrop,
                Poster = ti.Poster,
                MediaId = ti.MediaId,
                _colorPalette = ti._colorPalette,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Recommendations stored");
        await Task.CompletedTask;
    }

    private async Task StoreVideos()
    {
        try
        {
            List<Media> videos = Show?.Videos.Results.ToList()
                .ConvertAll<Media>(x => new Media(x, Show, "video")) ?? [];

            await _mediaContext.Medias.UpsertRange(videos)
                .On(v => new { v.Src, v.TvId })
                .WhenMatched((ts, ti) => new Media
                {
                    Id = ti.Id,
                    Src = ti.Src,
                    Iso6391 = ti.Iso6391,
                    Type = ti.Type,
                    TvId = ti.TvId,
                    Name = ti.Name,
                    Site = ti.Site,
                    Size = ti.Size,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        Logger.MovieDb($@"TvShow {Show?.Name}: Videos stored");
        await Task.CompletedTask;
    }

    public static async Task StoreImages(int id)
    {
        TvClient tvClient = new(id);
        TvShowAppends? show = await tvClient.WithAllAppends();
        if (show is null) return;

        List<Image> posters = show.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image: image, show: show, type: "poster"));

        List<Image> backdrops = show.Images.Backdrops.ToList()
            .ConvertAll<Image>(image => new Image(image: image, show: show, type: "backdrop"));

        List<Image> logos = show.Images.Logos.ToList()
            .ConvertAll<Image>(image => new Image(image: image, show: show, type: "logo"));

        List<Image> images = posters
            .Concat(backdrops)
            .Concat(logos)
            .ToList();

        await using MediaContext mediaContext = new();
        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.TvId })
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
                TvId = ti.TvId,
            })
            .RunAsync();
        
        try
        {
            var colorPaletteJob = new ColorPaletteJob(id: id, model: "tv");
            JobDispatcher.Dispatch(colorPaletteJob, "queue", 2);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        try
        {
            var colorPaletteJob = new ColorPaletteJob(id: id, model: "similar", type: "tv");
            JobDispatcher.Dispatch(colorPaletteJob, "queue", 2);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        try
        {
            var colorPaletteJob = new ColorPaletteJob(id: id, model: "recommendation", type: "tv");
            JobDispatcher.Dispatch(colorPaletteJob, "queue", 2);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        foreach (Image image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            try
            {
                var colorPaletteJob = new ColorPaletteJob(filePath: image.FilePath, model: "image", image.Iso6391);
                JobDispatcher.Dispatch(colorPaletteJob, "queue", 2);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }

        Logger.MovieDb($@"TvShow {show.Name}: Images stored");
        await Task.CompletedTask;
    }

    private async Task StoreNetworks()
    {
        // List<Keyword> keywords = Show?.Networks.Results.ToList()
        //     .ConvertAll<Network>(x => new Network(x)).ToArray() ?? [];
        //
        // await _mediaContext.Networks.UpsertRange(keywords)
        //     .On(v => new { v.Id })
        //     .WhenMatched((ts, ti) => new Network
        //     {
        //         Id = ti.Id,
        //         Name = ti.Name,
        //     })
        //     .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Networks stored");
        await Task.CompletedTask;
    }

    private async Task StoreCompanies()
    {
        // List<Company> companies = Show?.ProductionCompanies.Results.ToList()
        //     .ConvertAll<ProductionCompany>(x => new ProductionCompany(x)).ToArray() ?? [];
        //
        // await _mediaContext.Companies.UpsertRange(companies)
        //     .On(v => new { v.Id })
        //     .WhenMatched((ts, ti) => new ProductionCompany
        //     {
        //         Id = ti.Id,
        //         Name = ti.Name,
        //     })
        //     .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Companies stored");
        await Task.CompletedTask;
    }

    private async Task StoreKeywords()
    {
        Keyword[] keywords = Show?.Keywords.Results.ToList()
            .ConvertAll<Keyword>(x => new Keyword(x)).ToArray() ?? [];

        await _mediaContext.Keywords.UpsertRange(keywords)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Keyword
            {
                Id = ti.Id,
                Name = ti.Name,
            })
            .RunAsync();

        KeywordTv[] keywordTvs = Show?.Keywords.Results.ToList()
            .ConvertAll<KeywordTv>(x => new KeywordTv(x, Show)).ToArray() ?? [];

        await _mediaContext.KeywordTv.UpsertRange(keywordTvs)
            .On(v => new { v.KeywordId, v.TvId })
            .WhenMatched((ts, ti) => new KeywordTv
            {
                KeywordId = ti.KeywordId,
                TvId = ti.TvId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Keywords stored");
        await Task.CompletedTask;
    }

    private async Task StoreGenres()
    {
        GenreTv[] genreTvs = Show?.Genres.ToList()
            .ConvertAll<GenreTv>(x => new GenreTv(x, Show)).ToArray() ?? [];

        await _mediaContext.GenreTv.UpsertRange(genreTvs)
            .On(v => new { v.GenreId, v.TvId })
            .WhenMatched((ts, ti) => new GenreTv
            {
                GenreId = ti.GenreId,
                TvId = ti.TvId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show?.Name}: Genres stored");
        await Task.CompletedTask;
    }

    private string GetMediaType()
    {
        if (Show is null) return "";
        
        bool isAnime = KitsuIo.IsAnime(Show.Name, Show.FirstAirDate.ParseYear()).Result;

        return isAnime ? "anime" : "tv";
    }

    private string FindFolder()
    {
        return Show == null ? "" : FileNameParsers.CreateBaseFolder(Show);
    }

    private async Task Store()
    {
        if (Show == null) return;

        Tv tvResponse = new Tv(Show, library.Id, Folder, MediaType);

        await _mediaContext.Tvs.Upsert(tvResponse)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Tv
            {
                Id = ti.Id,
                Backdrop = ti.Backdrop,
                Duration = ti.Duration,
                FirstAirDate = ti.FirstAirDate,
                Homepage = ti.Homepage,
                ImdbId = ti.ImdbId,
                InProduction = ti.InProduction,
                LastEpisodeToAir = ti.LastEpisodeToAir,
                NextEpisodeToAir = ti.NextEpisodeToAir,
                NumberOfEpisodes = ti.NumberOfEpisodes,
                NumberOfSeasons = ti.NumberOfSeasons,
                OriginCountry = ti.OriginCountry,
                OriginalLanguage = ti.OriginalLanguage,
                Overview = ti.Overview,
                Popularity = ti.Popularity,
                Poster = ti.Poster,
                SpokenLanguages = ti.SpokenLanguages,
                Status = ti.Status,
                Tagline = ti.Tagline,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Trailer = ti.Trailer,
                TvdbId = ti.TvdbId,
                Type = ti.Type,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Folder = ti.Folder,
                LibraryId = ti.LibraryId,
                MediaType = ti.MediaType,
                _colorPalette = ti._colorPalette,
                UpdatedAt = ti.UpdatedAt,
            })
            .RunAsync();

        await _mediaContext.LibraryTv.Upsert(new LibraryTv(library.Id, Show.Id))
            .On(v => new { v.LibraryId, v.TvId })
            .WhenMatched((lts, lti) => new LibraryTv
            {
                LibraryId = lti.LibraryId,
                TvId = lti.TvId,
            })
            .RunAsync();

        Logger.MovieDb($@"TvShow {Show.Name}: TvShow stored");
    }

    private Task DispatchJobs()
    {
        if (Show == null) return Task.CompletedTask;

        try
        {
            FindMediaFilesJob findMediaFilesJob = new FindMediaFilesJob(id: Show.Id, libraryId: library.Id.ToString());
            JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        try
        {
            PersonJob personJob = new PersonJob(id: Show.Id, type: "tv");
            JobDispatcher.Dispatch(personJob, "queue", 3);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        try
        {
            TvImagesJob imagesJob = new TvImagesJob(id: Show.Id);
            JobDispatcher.Dispatch(imagesJob, "data", 2);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        return Task.CompletedTask;
    }

    public static async Task GetPalette(int id, bool download = true)
    {
        await using MediaContext mediaContext = new();

        Tv show = mediaContext.Tvs
            .First(e => e.Id == id);

        lock (show)
        {
            if (show is { _colorPalette: "" })
            {
                string palette = ImageLogic.GenerateColorPalette(posterPath: show.Poster, backdropPath: show.Backdrop).Result;
                show._colorPalette = palette;
                
                mediaContext.SaveChanges();
            }
        }
        
    }

    public static async Task GetSimilarPalette(int id)
    {
        await using MediaContext mediaContext = new();

        List<Similar>? similarList = await mediaContext.Similar
            .Where(e => e.TvFromId == id)
            .ToListAsync();

        if (similarList.Count is 0) return;

        foreach (Similar similar in similarList)
        {
            if (similar is not { _colorPalette: "" }) continue;
            
            string palette = ImageLogic
                .GenerateColorPalette(posterPath: similar.Poster, backdropPath: similar.Backdrop).Result;
            similar._colorPalette = palette;
        }
        await mediaContext.SaveChangesAsync();
    }

    public static async Task GetRecommendationPalette(int id)
    {
        await using MediaContext mediaContext = new();

        List<Recommendation> recommendations = await mediaContext.Recommendations
            .Where(e => e.TvFromId == id)
            .ToListAsync();

        if (recommendations.Count is 0) return;

        foreach (Recommendation recommendation in recommendations)
        {
            if (recommendation is not { _colorPalette: "" }) continue;
            
            string palette = ImageLogic.GenerateColorPalette(posterPath: recommendation.Poster,
                backdropPath: recommendation.Backdrop).Result;
            recommendation._colorPalette = palette;
        }
        
        await mediaContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Show = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}