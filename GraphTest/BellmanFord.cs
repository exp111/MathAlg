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
    internal class BellmanFord
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\");
        }

        [Test]
        public void TestWege1()
        {
            Assert.AreEqual("6.00", BellmanFordTest(@"path\Wege1", 2, 0)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWege2()
        {
            Assert.AreEqual("2.00", BellmanFordTest(@"path\Wege2", 2, 0)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWege3()
        {
            Assert.AreEqual(false, BellmanFordTest(@"path\Wege3", 2, 0).HasValue);
        }

        [Test]
        public void TestG_1_2()
        {
            Assert.AreEqual("5.56283", BellmanFordTest(@"weighted\G_1_2", 0, 1)!.Value.ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestG_1_2Undirected()
        {
            Assert.AreEqual("2.36802", BellmanFordTest(@"weighted\G_1_2", 0, 1, false)!.Value.ToString("0.00000", CultureInfo.InvariantCulture));
        }

        // Returns the length of the path or null if there is no path
        private double? BellmanFordTest(string fileName, int startID, int endID, bool directed = true)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var file = $"{fileName}.txt";
                Graph graph = Graph.FromTextFile(file, directed);
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var (tree, cycle) = graph.BellmanFord(startID);
                //TODO: get path to endID
                double? weight;
                if (tree != null)
                {
                    weight = tree.GetShortestPathWeight(endID);
                    var path = tree.GetShortestPath(endID)!;
                    Console.WriteLine($"Bellman-Ford: {path.GetPath()} ({weight})");
                }
                else
                {
                    weight = null;
                    Console.WriteLine($"Bellman-Ford: Found negative cycle at {cycle!}!");
                }
                stopwatch.Stop();
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
                var execTime = time - readTime;
                Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
                return weight;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during BellmanFordTest: {ex}");
            }
            return 0;
        }
    }
}
