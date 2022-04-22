using BenchmarkDotNet.Attributes;

namespace Graphen
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmark
    {
        private static readonly string path = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\";
        private static readonly string file = $"{path}/{fileName}.txt";

        private static readonly string fileName = "G_1_2";

        [Benchmark]
        public double BenchmarkBase()
        {
            var graph = Graph.FromTextFile(file);
            var res = graph.Prim();
            return res;
        }

        [Benchmark]
        public double BenchmarkText()
        {
            var graph = Graph.FromTextFile(file);
            return graph.Prim();
        }
    }
}
