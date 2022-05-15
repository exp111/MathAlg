using Graphen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTest
{
    internal class ReadFromFile
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraphSmall()
        {
            ReadFromFileTest("Graph1", 15, 17);
        }

        [Test]
        public void TestGraphMedium()
        {
            ReadFromFileTest("Graph2", 1000, 3000);
        }

        [Test]
        public void TestGraphMedium2()
        {
            ReadFromFileTest("Graph3", 1000, 2501);
        }

        [Test]
        public void TestGraphBig()
        {
            ReadFromFileTest("Graph_gross", 100000, 300000);
        }

        [Test]
        public void TestGraphBig2()
        {
            ReadFromFileTest("Graph_ganzgross", 500000, 1000000);
        }

        [Test]
        public void TestGraphBig3()
        {
            ReadFromFileTest("Graph_ganzganzgross", 1000000, 4000000);
        }

        [Test]
        public void TestWeightedGraph1k2k()
        {
            var graph = ReadFromFileWeightedTest("G_1_2", 1000, 2000, 1013.94);
            Assert.IsTrue(graph.KantenAnzahl > 0); // so we didnt get a null graph
            var first = graph.Kanten.First();
            Assert.IsNotNull(first);
            Assert.AreEqual(0, first.Start.ID);
            Assert.AreEqual(753, first.Ende.ID);
            Assert.AreEqual(0.02075, first.Weight);

            var last = graph.Kanten.Last();
            Assert.IsNotNull(last);
            Assert.AreEqual(805, last.Start.ID);
            Assert.AreEqual(538, last.Ende.ID);
            Assert.AreEqual(0.2668, last.Weight);
        }

        [Test]
        public void TestWeightedGraph1k20k()
        {
            ReadFromFileWeightedTest("G_1_20", 1000, 20000, 10062.66);
        }

        [Test]
        public void TestWeightedGraph1k200k()
        {
            ReadFromFileWeightedTest("G_1_200", 1000, 200000, 100230.43);
        }

        [Test]
        public void TestWeightedGraph10k20k()
        {
            ReadFromFileWeightedTest("G_10_20", 10000, 20000, 10070.33);
        }

        [Test]
        public void TestWeightedGraph10k200k()
        {
            ReadFromFileWeightedTest("G_10_200", 10000, 200000, 100276.18);
        }

        [Test]
        public void TestWeightedGraph100k200k()
        {
            ReadFromFileWeightedTest("G_100_200", 100000, 200000, 100134.53);
        }

        private void ReadFromFileTest(string fileName, int nodes, int edges)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile(@$"graph\{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                Assert.AreEqual(nodes, graph.KnotenAnzahl);
                Assert.AreEqual(edges, graph.KantenAnzahl);
                var readTime = stopwatch.Elapsed;
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during ReadFromFileTest: {ex}");
            }
        }

        private Graph ReadFromFileWeightedTest(string fileName, int nodes, int edges, double weight)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile(@$"weighted\{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                Assert.AreEqual(nodes, graph.KnotenAnzahl);
                Assert.AreEqual(edges, graph.KantenAnzahl);
                var edgeWeight = 0d;
                foreach (var edge in graph.Kanten)
                {
                    edgeWeight += edge.Weight!.Value;
                }
                Assert.AreEqual(weight.ToString("0.00", CultureInfo.InvariantCulture), weight.ToString("0.00", CultureInfo.InvariantCulture));

                var readTime = stopwatch.Elapsed;
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
                return graph;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during ReadFromFileTest: {ex}");
            }
            return new Graph(0);
        }
    }
}
