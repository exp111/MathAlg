#define MEASURE
//#define EXPORT
using Graphen;
using System.Diagnostics;

Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
var files = new List<string>();
//files.Add(@"weighted\G_1_2");
//files.Add(@"weighted\G_15_18");
//files.Add(@"weighted\G_1_20");
//files.Add(@"weighted\G_1_200");
//files.Add(@"weighted\G_10_20");
//files.Add(@"weighted\G_10_200");
//files.Add(@"weighted\G_100_200");

//files.Add(@"complete\K_10");
//files.Add(@"complete\K_10e");
//files.Add(@"complete\K_12");
//files.Add(@"complete\K_12e");
//files.Add(@"complete\K_15");
//files.Add(@"complete\K_15e");
//files.Add(@"complete\K_20");
//files.Add(@"complete\K_30");
//files.Add(@"complete\K_50");
//files.Add(@"complete\K_100");

foreach (var fileName in files)
{
    try
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine($"Reading {fileName}");
        var graph = Graph.FromTextFile($"{fileName}.txt");
        Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
        var readTime = stopwatch.Elapsed;
        var edges = graph.NearestNeighbour();
        Console.WriteLine($"NearestNeighbour: {edges.GetPath()} ({edges.GetWeight()})");
        var neighbourTime = stopwatch.Elapsed - readTime;

        var edgesDouble = graph.DoubleTree();
        Console.WriteLine($"DoubleTree: {edgesDouble.GetPath()} ({edgesDouble.GetWeight()})");
        var doubleTime = stopwatch.Elapsed - neighbourTime - readTime;

        var bruteforce = graph.BruteForceTSP();
        Console.WriteLine($"Bruteforce: {bruteforce.GetPath()} ({bruteforce.GetWeight()})");
        var bruteTime = stopwatch.Elapsed - doubleTime - neighbourTime - readTime;

        var branch = graph.BranchBoundTSP();
        Console.WriteLine($"BrandNBound: {branch.GetPath()} ({branch.GetWeight()})");
        var branchTime = stopwatch.Elapsed - bruteTime - doubleTime - neighbourTime - readTime;

        stopwatch.Stop();
        var time = stopwatch.Elapsed;
        //Debug.Assert(weightDouble == weight);
        Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
        Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
        var execTime = time - readTime;
        Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
        Console.WriteLine($"NearestNeighbour Time: {(int)neighbourTime.TotalMilliseconds} ms ({(int)neighbourTime.TotalSeconds} seconds)");
        Console.WriteLine($"DoubleTree Time: {(int)doubleTime.TotalMilliseconds} ms ({(int)doubleTime.TotalSeconds} seconds)");
        Console.WriteLine($"BruteForce Time: {(int)bruteTime.TotalMilliseconds} ms ({(int)bruteTime.TotalSeconds} seconds)");
        Console.WriteLine($"BrandNBound Time: {(int)branchTime.TotalMilliseconds} ms ({(int)branchTime.TotalSeconds} seconds)");
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

{
    /*var graph = Graph.FromTextFile(@"flows\Fluss.txt", true, true);
    File.WriteAllLines("out/Fluss.graphml", graph.ExportToGraphML());
    var a = graph.EdmondsKarp(0, 7);
    Console.WriteLine(a); //4

    graph = Graph.FromTextFile(@"flows\debug.txt", true, true);
    File.WriteAllLines("out/debug.graphml", graph.ExportToGraphML());
    a = graph.EdmondsKarp(0, 4);
    Console.WriteLine(a); //12
    graph = Graph.FromTextFile(@"flows\debug2.txt", true, true);
    File.WriteAllLines("out/debug2.graphml", graph.ExportToGraphML());
    a = graph.EdmondsKarp(5, 4);
    Console.WriteLine(a); //19*/
    //var graph = Graph.FromTextFile("G_D_4_4_cycle.txt", true);
    //graph.Dijkstra(0, 3);

    var graph = Graph.FromTextFileBalance(@"minimal_flows\Kostenminimal1.txt", true);
    var a = graph.CycleCanceling(); // 3
    Console.WriteLine(a);
    //TODO: test if negative cycle finding works
}