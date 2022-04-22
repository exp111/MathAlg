using BenchmarkDotNet.Attributes;

namespace Graphen
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmark
    {
        private static readonly string fileName = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\Graph_ganzganzgross.txt";

        [Benchmark]
        public void BenchmarkRead()
        {
            var graph = Graph.FromTextFile(fileName);
        }

        [Benchmark]
        public void BenchmarkText()
        {
            var graph = Graph.FromTextFile(fileName);
        }
    }
}
