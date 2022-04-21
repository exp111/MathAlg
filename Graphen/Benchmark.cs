using BenchmarkDotNet.Attributes;

namespace Graphen
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmark
    {
        private static readonly string fileName = @"E:\D\Visual Studio\Uni\MathAlg\Graphen\Graphen\bin\Release\net6.0\Graph_ganzganzgross.txt";

        [Benchmark]
        public void BenchmarkRead()
        {
            var file = File.OpenRead(fileName);
            var graph = Graph.FromTextFile(file);
        }

        [Benchmark]
        public void BenchmarkText()
        {
            var file = File.OpenRead(fileName);
            var graph = Graph.FromTextFile(file);
        }
    }
}
