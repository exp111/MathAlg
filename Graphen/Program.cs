//#define INFO
//#define EXPORT
#define MEASURE
using Graphen;
using System.Diagnostics;


//var files = new string[] {"Graph1", "Graph2", "Graph3"};
var files = new string[] { "Graph_gross", "Graph_ganzgross", "Graph_ganzganzgross" };
foreach (var fileName in files)
{
    try
    {
#if MEASURE
        var stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        var file = File.ReadAllText($"{fileName}.txt");
        var graph = Graphen.Graph.FromTextFile(file);
        Console.WriteLine($"Read {fileName}");
#if PRINT
        Console.WriteLine(graph);
#endif
        Console.WriteLine($"Zusammenhangskomponenten: {graph.GetZusammenhangskomponenten()}");
#if EXPORT
        File.WriteAllLines($"{fileName}.graphml", graph.ExportToGraphML());
        Console.WriteLine($"Graph written to {fileName}.graphml");
#endif
#if MEASURE
        stopwatch.Stop();
        Console.WriteLine($"{fileName} took {stopwatch.ElapsedMilliseconds} ms ({stopwatch.Elapsed.Seconds} seconds)");
#endif
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception during Main: {ex}");
    }
}