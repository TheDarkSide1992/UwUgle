using System.Diagnostics;
using EasyNetQ;
using Infrastructure.Implementations;
using Service.Implementations;
using Service.Interfaces;


IService _service;
_service = new ReaderService(new Reader());
    

using (var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest"))
{
    /*bus.PubSub.Subscribe<string>("strings", async (message) => Console.WriteLine(message));
    var input = string.Empty;
    Console.WriteLine("Enter a message. 'Quit' to quit.");
    while ((input = Console.ReadLine()) != "Quit")
    {
        await bus.PubSub.PublishAsync("test");
        Console.WriteLine("Message published!");
    }*/
}

    
string rootFolderPath = @"C:\Users\emilw\Downloads\enron_mail_20150507.tar\enron_mail_20150507\maildir";

var stopwatch = Stopwatch.StartNew();
_service.ReadFoldersSequentiallyWithParallelFilesAsBytes(rootFolderPath);
stopwatch.Stop();

Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds} ms");



//Console.WriteLine($"Total folders processed: {filesContent.Count}");




/*
// Display results
foreach (var folder in filesContent)
{
    Console.WriteLine($"\nFolder: {folder.Key}");
    foreach (var file in folder.Value)
    {
        Console.WriteLine($"  - {file.Key} ({file.Value.Length} bytes)");
    }
} */   
