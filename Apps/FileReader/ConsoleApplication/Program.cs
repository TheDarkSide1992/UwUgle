using System.Diagnostics;
using Infrastructure.Implementations;
using Service.Implementations;
using Service.Interfaces;


IService _service;
_service = new ReaderService(new Reader());

    
string rootFolderPath = @"C:\Users\emilw\Downloads\enron_mail_20150507.tar\enron_mail_20150507\maildir";

var stopwatch = Stopwatch.StartNew();
_service.ReadFoldersSequentiallyWithParallelFilesAsBytes(rootFolderPath);
stopwatch.Stop();

Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds} ms");

