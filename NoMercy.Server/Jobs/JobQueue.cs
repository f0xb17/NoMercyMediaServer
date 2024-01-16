using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.Jobs;

public class JobQueue
{
    private readonly MediaContext _context;
    private readonly int _maxAttempts;

    public JobQueue(MediaContext context, int maxAttempts = 3)
    {
        _context = context;
        _maxAttempts = maxAttempts;
    }

    public void Enqueue(Job job)
    {
        lock (_context)
        {
            _context.Jobs.Add(job);
            _context.SaveChanges();
        }
    }

    public Job? Dequeue()
    {
        lock (_context)
        {
            var job = _context.Jobs.FirstOrDefault();
            if (job != null)
            {
                _context.Jobs.Remove(job);
                _context.SaveChanges();
            }

            return job;
        }
    }

    public Job? ReserveJob(string name, long? currentJobId)
    {
        lock (_context)
        {
            Job? job = _context.Jobs
                .Where(j => j.ReservedAt == null && j.Attempts <= _maxAttempts)
                .Where(j => currentJobId == null)
                .Where(j => j.Queue == name)
                .OrderBy(j => j.CreatedAt)
                .FirstOrDefault() ?? null;

            if (job != null)
            {
                job.ReservedAt = DateTime.UtcNow;
                job.Attempts++;
                _context.SaveChanges();
            }

            return job;
        }
    }

    public void FailJob(Job job, Exception exception)
    {
        lock (_context)
        {
            job.ReservedAt = null;

            if (job.Attempts >= _maxAttempts)
            {
                var failedJob = new FailedJob
                {
                    Uuid = Guid.NewGuid(),
                    Connection = "default",
                    Queue = job.Queue,
                    Payload = job.Payload,
                    Exception = JsonConvert.SerializeObject(exception.InnerException ?? exception),
                    FailedAt = DateTime.UtcNow,
                };
                _context.Jobs.Remove(job);
                _context.FailedJobs.Add(failedJob);
            }

            _context.SaveChanges();
        }
    }

    public void DeleteJob(Job job)
    {
        lock (_context)
        {
            _context.Jobs.Remove(job);
            _context.SaveChanges();
        }
    }

    public void RequeueFailedJob(int failedJobId)
    {
        var failedJob = _context.FailedJobs.Find(failedJobId);
        if (failedJob != null)
        {
            lock (_context)
            {
                _context.FailedJobs.Remove(failedJob);
                _context.Jobs.Add(new Job
                {
                    Queue = failedJob.Queue,
                    Payload = failedJob.Payload,
                    AvailableAt = DateTime.UtcNow,
                    Attempts = 0,
                });
                _context.SaveChanges();
            }
        }
    }
}