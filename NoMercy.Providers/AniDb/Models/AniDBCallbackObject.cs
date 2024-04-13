#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using AniDB;
using AniDB.ResponseItems;

namespace NoMercy.Providers.AniDb.Models;

public class AniDbCallbackObject<T> : IAniDBMessageResponseCallback<T> where T : IAniDBResponseItem
{
    private readonly Action<T> _callback;
    public void Callback(T messageItem)
    {
        _callback(messageItem);
    }
    
    public AniDbCallbackObject(Action<T> callback)
    {
        _callback = callback;
    }
}