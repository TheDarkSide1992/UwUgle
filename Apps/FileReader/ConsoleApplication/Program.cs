using System.Diagnostics;
using Infrastructure.Implementations;
using Service.Implementations;
using Service.Interfaces;


public static class Program {

    public static async Task Main()
    {
        IService _service = new ReaderService(new Reader());

        // should contain 517.401 Files, 3.499 Folders
        string rootFolderPath = Environment.GetEnvironmentVariable("file_folder");

        var stopwatch = Stopwatch.StartNew();
        await Task.Run(() => _service.ReadFoldersSequentiallyWithParallelFilesAsBytes(rootFolderPath));
        stopwatch.Stop();

        Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds} ms");
    }

}

