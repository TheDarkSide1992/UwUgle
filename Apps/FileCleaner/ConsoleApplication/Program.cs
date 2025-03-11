// See https://aka.ms/new-console-template for more information

using Service;
using Service.Implementations;

Console.WriteLine("Hello, World!");

CleanerService cs = new CleanerService(new CleanerString());
cs.start();

