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
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\path");
        }

        [Test]
        public void TestWege1()
        {
            Assert.AreEqual("6.00", BellmanFordTest("Wege1", 2, 0)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWege2()
        {
            Assert.AreEqual("2.00", BellmanFordTest("Wege2", 2, 0)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWege3()
        {
            Assert.AreEqual(false, BellmanFordTest("Wege3", 2, 0).HasValue);
        }

        [Test]
        public void TestG_1_2()
        {
            Assert.AreEqual("5.56283", BellmanFordTest("Wege2", 0, 1)!.Value.ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestG_1_2Undirected()
        {
            Assert.AreEqual("2.36802", BellmanFordTest("Wege3", 0, 1, false)!.Value.ToString("0.00000", CultureInfo.InvariantCulture));
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
                Graph graph = directed ? Graph.FromTextFile(file) : Graph.FromTextFileDirected(file);
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var (edges, cycle) = graph.BellmanFord(startID);
                //TODO: get path to endID
                double? weight;
                if (!cycle)
                {
                    weight = edges.Kanten.GetWeight(); //TODO: get weight
                    Console.WriteLine($"Bellman-Ford: {edges} ({weight})");
                }
                else
                {
                    Console.WriteLine($"Bellman-Ford: Found negative cycle!");
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
                Console.WriteLine($"Exception during BruteForceTest: {ex}");
            }
            return 0;
        }
    }
}
