//#define INFO
//#define EXPORT
#define MEASURE
using Graphen;
using System.Diagnostics;


//var files = new string[] {"Graph1", "Graph2", "Graph3", "Graph_gross", "Graph_ganzgross" };
var files = new string[] { "Graph_ganzganzgross" };
foreach (var fileName in files)
{
    try
    {
#if MEASURE
        var stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        Console.WriteLine($"Reading {fileName}");
        var file = File.ReadAllText($"{fileName}.txt");
        var graph = Graph.FromTextFile(file);
        Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
#if MEASURE
        var readTime = stopwatch.Elapsed;
#endif
#if INFO
        Console.WriteLine(graph);
#endif
        Console.WriteLine($"Zusammenhangskomponenten: {graph.GetZusammenhangskomponenten()}");
#if EXPORT
        File.WriteAllLines($"{fileName}.graphml", graph.ExportToGraphML());
        Console.WriteLine($"Graph written to {fileName}.graphml");
#endif
#if MEASURE
        stopwatch.Stop();
        var time = stopwatch.Elapsed;
        Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
        Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
        var execTime = time - readTime;
        Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
#endif
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception during Main: {ex}");
    }
}