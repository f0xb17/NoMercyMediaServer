using System.Reflection;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Server.Jobs;

public interface IShouldQueue
{
    Task Handle();
}

public static class JobDispatcher
{
    private static readonly JobQueue Queue;

    static JobDispatcher()
    {
        var context = new MediaContext();
        Queue = new JobQueue(context);
    }
    
    public static void Dispatch(IShouldQueue job, string onQueue = "default")
    {
        var constructor = job.GetType().GetConstructors()[0];
        var constructorParameters = constructor.GetParameters();
        var constructorArguments = new object?[constructorParameters.Length];
        
        for (int i = 0; i < constructorParameters.Length; i++)
        {
            var parameter = constructorParameters[i];
            
            var field = job.GetType().GetField("_" + parameter.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                throw new Exception($"Field '{"_" + parameter.Name}' not found in type '{job.GetType().FullName}'.");
            }
        
            constructorArguments[i] = field.GetValue(job);
        }

        var fullName = job.GetType().FullName;
        if (fullName != null)
        {
            var payload = new Dictionary<string, string>
            {
                { "className", fullName },
                { "jobMethod", nameof(IShouldQueue.Handle)},
                { "jobParams", JsonConvert.SerializeObject(constructorArguments) }
            };

            var jobData = new Job
            {
                Queue = onQueue,
                Payload = JsonConvert.SerializeObject(payload),
                AvailableAt = DateTime.UtcNow,
            };

            Queue.Enqueue(jobData);
        }
    }

}