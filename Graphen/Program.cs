#define MEASURE
#define EXPORT
using BenchmarkDotNet.Running;
using Graphen;
using System.Diagnostics;

#if BENCHMARK
BenchmarkRunner.Run<Benchmark>();
#endif

Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
var files = new List<string>();
files.Add("G_1_2");
//files.Add("G_15_18");
//files.Add("G_1_20");
//files.Add("G_1_200");
//files.Add("G_10_20");
//files.Add("G_10_200");
//files.Add("G_100_200");

#if BENCHMARK
BenchmarkRunner.Run<Benchmark>();
#endif

foreach (var fileName in files)
{
    try
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine($"Reading {fileName}");
        var graph = Graph.FromTextFileWeighted($"{fileName}.txt");
        Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
        var readTime = stopwatch.Elapsed;

        Console.WriteLine($"Prim: {graph.Prim()}");
        Console.WriteLine($"Kruskal: {graph.Kruskal()}");

        stopwatch.Stop();
        var time = stopwatch.Elapsed;
        Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
        Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
        var execTime = time - readTime;
        Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
#if EXPORT
        File.WriteAllLines($"out/{fileName}.graphml", graph.ExportToGraphML());
        Console.WriteLine($"Graph written to {fileName}.graphml");
#endif
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception during Main: {ex}");
    }
}