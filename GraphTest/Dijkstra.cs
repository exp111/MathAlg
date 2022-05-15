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
    internal class Dijkstra
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\path");
        }

        [Test]
        public void TestWege1()
        {
            Assert.AreEqual("6.00", DijkstraTest("Wege1", 2, 0).ToString("0.00", CultureInfo.InvariantCulture));
        }

        // Wege2 and Wege3 have negative weights

        [Test]
        public void TestG_1_2()
        {
            Assert.AreEqual("5.56283", DijkstraTest("Wege2", 0, 1).ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestG_1_2Undirected()
        {
            Assert.AreEqual("2.36802", DijkstraTest("Wege3", 0, 1, false).ToString("0.00000", CultureInfo.InvariantCulture));
        }


        private double DijkstraTest(string fileName, int startID, int endID, bool directed = true)
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
                var edges = graph.Dijkstra(startID);
                //TODO: get path to endID
                var weight = edges.GetWeight();
                Console.WriteLine($"Dijkstra: {edges.GetPath()} ({weight})");
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
