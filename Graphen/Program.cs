//#define INFO
//#define EXPORT
#define MEASURE
using Graphen;
using System.Diagnostics;

#if INFO
var nodeTreshold = 50;
#endif


var files = new List<string>();
//files.Add("Graph1");
//files.Add("Graph2");
//files.Add("Graph3");
//files.Add("Graph_gross");
//files.Add("Graph_ganzgross");
files.Add("Graph_ganzganzgross");

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
        if (graph.KnotenAnzahl < nodeTreshold)
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