using BenchmarkDotNet.Attributes;
using Graphen;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
[RankColumn]
public class BenchmarkSmall
{
    private static readonly string path = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\complete";
    private static readonly string fileName = "K_12.txt";
    private static readonly string file = Path.Combine(path, fileName);

    [Benchmark(Baseline = true)]
    public List<Kante> BenchmarkBase()
    {
        var graph = Graph.FromTextFile(file);
        return graph.BranchBoundTSP();
    }

    [Benchmark]
    public List<Kante> BenchmarkB()
    {
        var graph = Graph.FromTextFile(file);
        return graph.BranchBoundTSPB();
    }
}

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
[RankColumn]
public class BenchmarkMedium
{
    private static readonly string path = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\graph";
    private static readonly string fileName = "G_10_200.txt";
    private static readonly string file = Path.Combine(path, fileName);

    [Benchmark(Baseline = true)]
    public Graph BenchmarkBase()
    {
        var graph = Graph.FromTextFile(file);
        return graph.Prim();
    }

    [Benchmark]
    public Graph BenchmarkB()
    {
        var graph = Graph.FromTextFile(file);
        return graph.PrimB();
    }
}

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
[RankColumn]
public class BenchmarkBig
{
    private static readonly string path = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\graph";
    private static readonly string fileName = "G_100_200.txt";
    private static readonly string file = Path.Combine(path, fileName);

    [Benchmark(Baseline = true)]
    public Graph BenchmarkBase()
    {
        var graph = Graph.FromTextFile(file);
        return graph.Prim();
    }

    [Benchmark]
    public Graph BenchmarkB()
    {
        var graph = Graph.FromTextFile(file);
        return graph.PrimB();
    }
}