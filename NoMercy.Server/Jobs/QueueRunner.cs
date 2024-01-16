using NoMercy.Database;

namespace NoMercy.Server.Jobs;

public static class Runner
{
    public static Task Start()
    {
        JobQueue jobQueue = new(new MediaContext());
        
        List<Task> taskList = new List<Task>();

        for (int i = 0; i < 15; i++)
        {
            int y = i;
            taskList.Add(Task.Run(() =>
            {
                Worker worker = new Worker(jobQueue, y);
                worker.Start();
            }));
        }
        
        Task.WhenAll(taskList);
        
        return Task.CompletedTask;
    }
}