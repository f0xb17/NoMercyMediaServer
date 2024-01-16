namespace NoMercy.Server.Jobs;

public class HiMistaaJob : IShouldQueue
{
    private readonly string _param1;
    private readonly string _param2;

    public HiMistaaJob(string param1, string param2)
    {
        _param1 = param1;
        _param2 = param2;
    }

    public new Task Handle()
    {
        Console.WriteLine($@"HIIIII {_param1} - {_param2}");
        return Task.CompletedTask;
    }
}