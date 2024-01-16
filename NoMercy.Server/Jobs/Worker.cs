using Newtonsoft.Json;
using NoMercy.Helpers;
using Exception = System.Exception;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Jobs;

public delegate void WorkCompletedEventHandler(object sender, EventArgs e);

public class Worker(JobQueue queue, string name = "default")
{
    private long? _currentJobId;
    private bool _isRunning = true;

    private int CurrentIndex
    {
        get => QueueRunner.GetWorkerIndex(name, this);
    }

    public event WorkCompletedEventHandler WorkCompleted = delegate { };

    public void Start()
    {
        Logger.Queue($"Worker {name} - {CurrentIndex}: started", LogLevel.Info);

        while (_isRunning)
        {
            // Logger.Queue($"Worker {id}: Checking for jobs in queue {name}...");
            var job = queue.ReserveJob(name, _currentJobId);

            if (job != null)
            {
                _currentJobId = job.Id;
                var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(job.Payload);
                if (payload != null)
                {
                    var className = Type.GetType(payload["className"]);
                    var jobMethod = className?.GetMethod(payload["jobMethod"]);
                    var jobParams = JsonHelper.FromJson<object?[]?>(payload["jobParams"]);

                    if (className != null)
                    {
                        try
                        {
                            var jobInstance = Activator.CreateInstance(className, jobParams);
                            Logger.Queue($"Worker {name} - {CurrentIndex}: Processing job {job.Id} of type {payload["className"]}...");

                            if (jobMethod != null) jobMethod.Invoke(jobInstance, null);
                            // If the job was processed successfully, delete it from the Jobs table
                            queue.DeleteJob(job);
                            _currentJobId = null;

                            OnWorkCompleted(EventArgs.Empty);

                            Logger.Queue(
                                $"Worker {name} - {CurrentIndex}: Job {job.Id} of type {payload["className"]} processed successfully.");
                        }
                        catch (Exception ex)
                        {
                            // If the job's Handle method throws an exception, add it to the FailedJobs table
                            queue.FailJob(job, ex);
                            _currentJobId = null;

                            Logger.Queue(
                                $"Worker {name} - {CurrentIndex}: Job {job.Id} of type {payload["className"]} failed with error: {ex.Message}");
                        }
                    }
                }

                Thread.Sleep(1000);
            }
            else
            {                            
                OnWorkCompleted(EventArgs.Empty);

                // If there are no jobs to process, sleep for a while before checking again
                Thread.Sleep(5000);
            }
        }
    }
    
    protected virtual void OnWorkCompleted(EventArgs e)
    {
        WorkCompleted?.Invoke(this, e);
    }

    public void Stop()
    {
        Logger.Queue($"Worker {name} - {CurrentIndex}: stopped", LogLevel.Info);
        _isRunning = false;
    }

    public void Restart()
    {
        Stop();
        Start();
    }

    public void StopWhenReady()
    {
        while (_currentJobId != null)
        {
            Thread.Sleep(1000);
        }

        Stop();
    }
}