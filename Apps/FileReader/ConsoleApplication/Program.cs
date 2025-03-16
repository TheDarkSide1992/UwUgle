using System.Diagnostics;
using Infrastructure.Implementations;
using Service.Implementations;
using Service.Interfaces;


public static class Program {

    public static async Task Main()
    {
        IService _service;
        _service = new ReaderService(new Reader());

        
        string rootFolderPath = Environment.GetEnvironmentVariable("file_folder");

        var stopwatch = Stopwatch.StartNew();
        _service.ReadFoldersSequentiallyWithParallelFilesAsBytes(rootFolderPath);
        stopwatch.Stop();

        Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds} ms");
    }

}

