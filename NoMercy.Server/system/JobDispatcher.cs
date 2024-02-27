using System.Reflection;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.system;

public interface IShouldQueue
{
    Task Handle();
}

public static class JobDispatcher
{
    private static readonly JobQueue Queue;

    static JobDispatcher()
    {
        var context = new QueueContext();
        Queue = new JobQueue(context);
    }
    
    public static void Dispatch(IShouldQueue job, string onQueue = "default", int priority = 0)
    {
        foreach (var constructor in job.GetType().GetConstructors())
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
            
            if(constructorArguments.Length > 0  && (constructorArguments[0] == null || constructorArguments[0] is 0))
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

            var jobData = new QueueJob()
            {
                Queue = onQueue,
                Payload = JsonConvert.SerializeObject(payload),
                AvailableAt = DateTime.UtcNow,
                Priority = priority
            };

            Queue.Enqueue(jobData);
        }
    }

}