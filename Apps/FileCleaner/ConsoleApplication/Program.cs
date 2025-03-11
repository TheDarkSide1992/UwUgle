// See https://aka.ms/new-console-template for more information

using Service;
using Service.Implementations;
using Service.Implementations.converter;

CleanerStringSpacing cleaner = new CleanerStringSpacing();
ByteArrayConverter converter = new ByteArrayConverter();

CleanerService cs = new CleanerService(cleaner, converter);

await cs.Start();



