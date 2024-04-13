using System.Reflection;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Server.system;

public interface IShouldQueue
{
    Task Handle();
}

public static class JobDispatcher
{
    private static readonly JobQueue Queue = new(new QueueContext());
    
    public static void Dispatch(IShouldQueue job, string onQueue = "default", int priority = 0, int attempt = -1)
    {
        foreach (var constructor in job.GetType().GetConstructors().OrderByDescending(ctor => ctor.GetParameters().Length))
        {
            var constructorParameters = constructor.GetParameters();
            var constructorArguments = new object?[constructorParameters.Length];
            
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];
                
                var field = job.GetType().GetField("_" + parameter.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
                if (field == null)
                {
                    throw new Exception($"Field '{"_" + parameter.Name}' not found in Type '{job.GetType().FullName}'.");
                }
            
                constructorArguments[i] = field.GetValue(job);
            }
            
            if(constructorArguments.Length > 0  && (constructorArguments[0] is null || constructorArguments[0] is 0 || constructorArguments.Contains(null)))
            {
                continue;
            }

            var fullName = job.GetType().FullName;
            if (fullName == null) continue;
            
            var payload = new Dictionary<string, string>
            {
                { "className", fullName },
                { "jobMethod", nameof(IShouldQueue.Handle)},
                { "jobParams", JsonConvert.SerializeObject(constructorArguments) }
            };
            
            string payloadString = JsonConvert.SerializeObject(payload);
            
            using QueueContext context = new();
            bool exists = context.QueueJobs.Any(queueJob => queueJob.Payload == payloadString);
            if (exists) return;

            var jobData = new QueueJob()
            {
                Queue = onQueue,
                Payload = payloadString,
                AvailableAt = DateTime.UtcNow,
                Priority = priority
            };

            try
            {
                Queue.Enqueue(jobData);
            }
            catch(Exception e)
            {
                Logger.Queue(e, Helpers.LogLevel.Error);
            }
        }
    }
}