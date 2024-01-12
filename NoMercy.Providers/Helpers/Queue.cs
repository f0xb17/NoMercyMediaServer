
using System.Collections.Concurrent;

namespace NoMercy.Providers.Helpers;

public enum State
{
    Idle,
    Running,
    Stopped
}

public class QueueOptions
{
    public int Concurrent { get; init; } = 5;
    public int Interval { get; init; } = 500;
    public bool Start { get; init; } = true;
}

public class QueueEventArgs : EventArgs
{
    public object? Result { get; set; }
    public Exception? Error { get; set; }
}

public class Queue(QueueOptions options)
{
    private readonly Dictionary<string, Func<Task>> _tasks = new();
    
    private int _lastRan;
    private int _currentlyHandled;
    
    private State _state = State.Idle;
    private QueueOptions Options { get; set; } = options;
    private SemaphoreSlim Semaphore { get; set; } = new(options.Concurrent, options.Concurrent);

    public event EventHandler<QueueEventArgs> Resolve = null!;
    public event EventHandler<QueueEventArgs> Reject = null!;
    public event EventHandler? Start;
    public event EventHandler? Stop;
    public event EventHandler? End;

    private void StartQueue()
    {
        if (_state == State.Running || IsEmpty) return;
        
        _state = State.Running;
        Start?.Invoke(this, EventArgs.Empty);
        RunTasks();
    }

    private void StopQueue()
    {
        _state = State.Stopped;
        Stop?.Invoke(this, EventArgs.Empty);
    }

    private void Finish()
    {
        _currentlyHandled--;

        if (_currentlyHandled != 0 || !IsEmpty) return;
        
        // StopQueue();
        _state = State.Idle;
        End?.Invoke(this, EventArgs.Empty);
    }

    private async void RunTasks()
    {
        while (ShouldRun)
        {
            await Dequeue();
        }
    }

    private Task Execute()
    {
        List<KeyValuePair<string, Func<Task>>> tasks = new List<KeyValuePair<string, Func<Task>>>(_tasks)
            .Where(_ => _currentlyHandled < Options.Concurrent).ToList();
        
        foreach (KeyValuePair<string, Func<Task>> task in tasks)
        {
            _currentlyHandled++;
            lock (_tasks)
                _tasks.Remove(task.Key);
                
            try
            {
                var result = task.Value.Invoke();
                Resolve?.Invoke(this, new QueueEventArgs { Result = result });
            }
            catch (Exception ex)
            {
                Reject?.Invoke(this, new QueueEventArgs { Error = ex });
            }
            finally
            {
                Finish();
            }
        }

        return Task.CompletedTask;
    }

    private Task Dequeue()
    {        
        var interval = Math.Max(0, Options.Interval - (Environment.TickCount - _lastRan));
        return Task.Run(async () =>
        {
            await Task.Delay(interval);
            _lastRan = Environment.TickCount;
            await Execute();
        });
    }

    public async Task<T> Enqueue<T>(Func<Task<T>> task, string? url)
    {
        await Semaphore.WaitAsync();
        
        var tcs = new TaskCompletionSource<T>();

        string uniqueId = Ulid.NewUlid().ToString();
        lock(_tasks)
            _tasks.Add(uniqueId, async () =>
            {
            try
            {
                var result = await task();
                Resolve?.Invoke(this, new QueueEventArgs { Result = result });
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Url failed: {0} {1}", url, ex.Message);
                Reject?.Invoke(this, new QueueEventArgs { Error = ex });
                tcs.SetException(ex);
            }
            finally
            {
                Semaphore.Release();
            }
            });

        if (Options.Start && _state != State.Stopped) StartQueue();

        return tcs.Task.Result;
    }

    private void Clear()
    {
        lock (_tasks)
            _tasks.Clear();
    }

    private int Size => _tasks.Count;

    private bool IsEmpty => Size == 0;

    private bool ShouldRun => !IsEmpty && _state != State.Stopped;
}

