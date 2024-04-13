#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Socket;

public class VideoHub : ConnectionHub
{
    public VideoHub(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }
    
    public async Task SetTime(VideoProgressRequest request)
    {
        Guid userId = Guid.Parse(Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        User? user = TokenParamAuthMiddleware.Users.FirstOrDefault(x => x.Id == userId);
        
        if(user is null)
        {
            return;
        }
        
        UserData userdata = new UserData
        {   
            UserId = user.Id,
            Type = request.PlaylistType,
            Time = request.Time,
            Audio = request.Audio,
            Subtitle = request.Subtitle,
            SubtitleType = request.SubtitleType,
            VideoFileId = Ulid.Parse(request.VideoId),
            MovieId = request.PlaylistType == "movie" ?  request.TmdbId : null,
            TvId = request.PlaylistType == "tv" ?  request.TmdbId : null,
            CollectionId = request.PlaylistType == "collection" ?  request.TmdbId : null,
            SpecialId = request.SpecialId,
        };
        
        await using MediaContext mediaContext = new();
        await mediaContext.UserData.Upsert(userdata)
            .On(x => new { x.UserId, x.VideoFileId })
            .WhenMatched((uds, udi) => new UserData
            {
                Id = uds.Id,
                Time = udi.Time,
                Audio = udi.Audio,
                Subtitle = udi.Subtitle,
                SubtitleType = udi.SubtitleType,
                LastPlayedDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            })
            .RunAsync();
    }
    
    public async Task RemoveWatched(VideoProgressRequest request){
        Guid userId = Guid.Parse(Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        User? user = TokenParamAuthMiddleware.Users.FirstOrDefault(x => x.Id == userId);
        
        if(user is null)
        {
            return;
        }
        
        await using MediaContext mediaContext = new();
        UserData[] userdata = await mediaContext.UserData
            .Where(x => x.UserId == user.Id)
            .Where(x => x.Type == request.PlaylistType)
            .Where(x => x.MovieId == request.TmdbId 
                        || x.TvId == request.TmdbId 
                        || x.SpecialId == request.SpecialId 
                        || x.CollectionId == request.TmdbId)
            .ToArrayAsync();
        
        mediaContext.UserData.RemoveRange(userdata);
        
        await mediaContext.SaveChangesAsync();
    }
    
    public Task CreateSession(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task Disconnect(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task Connect(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task Party(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task JoinSession(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task LeaveSession(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task PartyTime(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task PartyPlay(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task PartyPause(VideoProgressRequest request){
        return Task.CompletedTask;
    }
    public Task PartyItem(VideoProgressRequest request){
        return Task.CompletedTask;
    }

    public class VideoProgressRequest
    {
        [JsonProperty("app")] public int AppId { get; set; }
        [JsonProperty("video_id")] public string VideoId { get; set; }
        [JsonProperty("tmdb_id")] public int TmdbId { get; set; }
        [JsonProperty("playlist_type")] public string PlaylistType { get; set; }
        [JsonProperty("video_type")] public string VideoType { get; set; }
        [JsonProperty("time")] public int Time { get; set; }
        [JsonProperty("audio")] public string Audio { get; set; }
        [JsonProperty("subtitle")] public string Subtitle { get; set; }
        [JsonProperty("subtitle_type")] public string SubtitleType { get; set; }
        [JsonProperty("special_id")] public Ulid? SpecialId { get; set; }
    }
}

