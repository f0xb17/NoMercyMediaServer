using NoMercy.Database;
using NoMercy.Helpers;
using LogLevel = NoMercy.Helpers.LogLevel;

// ReSharper disable All

namespace NoMercy.Server.system;

public static class QueueRunner
{
    private static readonly Dictionary<string, (int count, List<Worker> workerInstances, CancellationTokenSource _cancellationTokenSource)> Workers = new()
    {
        ["cron"] = (5, [], new CancellationTokenSource()),
        ["queue"] = (1, [], new CancellationTokenSource()),
        ["data"] = (30, [], new CancellationTokenSource()),
        ["encoder"] = (2, [], new CancellationTokenSource()),
        // ["request"] = (15, [], new CancellationTokenSource()),
    };

    private static bool _isInitialized;
    private static readonly JobQueue JobQueue = new(new QueueContext());
    private static bool _isUpdating;
    
    public static Task Initialize()
    {
        if (_isInitialized) return Task.CompletedTask;

        _isInitialized = true;

        List<Task> taskList = [];

        foreach (var keyValuePair in Workers)
        {
            for (int i = 0; i < keyValuePair.Value.count; i++)
            {
                taskList.Add(Task.Run(() => SpawnWorker(keyValuePair.Key)));
            }
        }
        
        Task.WhenAll(taskList);

        return Task.CompletedTask;
    }

    private static Task SpawnWorker(string name)
    {
        Worker workerInstance = new Worker(JobQueue, name);

        workerInstance.WorkCompleted += QueueWorkerCompleted(name, workerInstance);

        Workers[name].workerInstances.Add(workerInstance);

        workerInstance.Start();
        
        return Task.CompletedTask;
    }

    #region MyRegion
    
    public static Task Start(string name)
    {
        foreach (var workerInstance in Workers[name].workerInstances)
        {
            workerInstance.Start();
        }

        return Task.CompletedTask;
    }

    public static Task StartAll()
    {
        foreach (var keyValuePair in Workers)
        {
            Start(keyValuePair.Key);
        }

        return Task.CompletedTask;
    }

    public static Task Stop(string name)
    {
        foreach (var workerInstance in Workers[name].workerInstances)
        {
            workerInstance.Stop();
        }

        return Task.CompletedTask;
    }

    public static Task StopAll()
    {
        foreach (var keyValuePair in Workers)
        {
            Stop(keyValuePair.Key);
        }

        return Task.CompletedTask;
    }

    public static Task Restart(string name)
    {
        foreach (var workerInstance in Workers[name].workerInstances)
        {
            workerInstance.Restart();
        }

        return Task.CompletedTask;
    }

    public static Task RestartAll()
    {
        foreach (var keyValuePair in Workers)
        {
            Restart(keyValuePair.Key);
        }

        return Task.CompletedTask;
    }

    
    #endregion
    
    
    private static WorkCompletedEventHandler QueueWorkerCompleted(string name, Worker instance)
    {
        return (sender, args) =>
        {
            if (!ShouldRemoveWorker(name)) return;
            
            instance.Stop();
            Workers[name].workerInstances.Remove(instance);
        };
    }

    private static bool ShouldRemoveWorker(string name)
    {
        return Workers[name].workerInstances.Count > Workers[name].count;
    }

    private static Task UpdateRunningWorkerCounts(string name)
    {
        if (ShouldRemoveWorker(name))
        {
            return Task.CompletedTask;
        }

        int i = Workers[name].workerInstances.Count;
        
        Task.Run(async () =>
        {
            while (!_isUpdating  && i < Workers[name].count)
            {
                if (_isUpdating || i >= Workers[name].count) break;
                
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => SpawnWorker(name));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                
                i += 1;
                
                await Task.Delay(100);
            }
        }, cancellationToken: Workers[name]._cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public static async Task<bool> SetWorkerCount(string name, int max)
    {
        if (!Workers.ContainsKey(name)) return false;
        
        Logger.Queue($"Setting queue {name} to {max} workers", LogLevel.Info);
        _isUpdating = true;
        Workers[name]._cancellationTokenSource.Cancel();
            
        var valueTuple = Workers[name];
        valueTuple.count = max;
        valueTuple._cancellationTokenSource = new CancellationTokenSource();
        Workers[name] = valueTuple;
            
        await Task.Run(() => {
            _isUpdating = false;
            UpdateRunningWorkerCounts(name);
        }, Workers[name]._cancellationTokenSource.Token);
        
        return true;

    }
    
    public static int GetWorkerIndex(string name, Worker worker)
    {
        return Workers[name].workerInstances.IndexOf(worker);
    }
}