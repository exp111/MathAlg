using BenchmarkDotNet.Attributes;
using Graphen;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Benchmark
{
    private static readonly string path = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\";
    private static readonly string fileName = "G_10_200.txt";
    private static readonly string file = Path.Combine(path, fileName);

    [Benchmark(Baseline = true)]
    public Graph BenchmarkBase()
    {
        var graph = Graph.FromTextFileWeighted(file);
        return graph.Kruskal();
    }

    [Benchmark]
    public Graph BenchmarkB()
    {
        var graph = Graph.FromTextFileWeighted(file);
        return graph.KruskalB();
    }
}