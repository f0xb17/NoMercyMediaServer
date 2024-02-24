using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Queue.Models;
// ReSharper disable All

namespace NoMercy.Queue.system;

public class JobQueue
{
    private readonly QueueContext _context;
    private readonly int _maxAttempts;

    public JobQueue(QueueContext context, int maxAttempts = 3)
    {
        _context = context;
        _maxAttempts = maxAttempts;
    }

    public void Enqueue(QueueJob queueJob)
    {
        lock (_context)
        {
            _context.QueueJobs.Add(queueJob);
            _context.SaveChanges();
        }
    }

    public QueueJob? Dequeue()
    {
        lock (_context)
        {
            var job = _context.QueueJobs.FirstOrDefault();
            if (job == null) return job;
            
            _context.QueueJobs.Remove(job);
            _context.SaveChanges();

            return job;
        }
    }

    public QueueJob? ReserveJob(string name, long? currentJobId)
    {
        lock (_context)
        {
            QueueJob? job = _context.QueueJobs
                .Where(j => j.ReservedAt == null && j.Attempts <= _maxAttempts)
                .Where(j => currentJobId == null)
                .Where(j => j.Queue == name)
                .OrderByDescending(j => j.Priority)
                .FirstOrDefault() ?? null;

            if (job == null) return job;
            
            job.ReservedAt = DateTime.UtcNow;
            job.Attempts++;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Queue(e, LogLevel.Error);
            }

            return job;
        }
    }

    public void FailJob(QueueJob queueJob, Exception exception)
    {
        lock (_context)
        {
            queueJob.ReservedAt = null;

            if (queueJob.Attempts >= _maxAttempts)
            {
                var failedJob = new FailedJob
                {
                    Uuid = Guid.NewGuid(),
                    Connection = "default",
                    Queue = queueJob.Queue,
                    Payload = queueJob.Payload,
                    Exception = JsonConvert.SerializeObject(exception.InnerException ?? exception),
                    FailedAt = DateTime.UtcNow
                };
                _context.QueueJobs.Remove(queueJob);
                _context.FailedJobs.Add(failedJob);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Queue(e, LogLevel.Error);
            }
        }
    }

    public void DeleteJob(QueueJob queueJob)
    {
        lock (_context)
        {
            _context.QueueJobs.Remove(queueJob);
            _context.SaveChanges();
        }
    }

    public void RequeueFailedJob(int failedJobId)
    {
        lock (_context)
        {
            var failedJob = _context.FailedJobs.Find(failedJobId);
            if (failedJob == null) return;
            
            _context.FailedJobs.Remove(failedJob);
            _context.QueueJobs.Add(new QueueJob
            {
                Queue = failedJob.Queue,
                Payload = failedJob.Payload,
                AvailableAt = DateTime.UtcNow,
                Attempts = 0
            });
            _context.SaveChanges();
        }
    }
}