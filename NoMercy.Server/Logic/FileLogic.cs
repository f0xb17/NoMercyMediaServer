using MovieFileLibrary;
using NoMercy.Database;

namespace NoMercy.Server.Logic;

public class FileLogic
{
    private readonly MediaContext _mediaContext = new();
    private readonly MovieDetector _movieDetector = new();
    
     public void Dispose()
     {
         GC.Collect();
         GC.WaitForFullGCComplete();
     }
}