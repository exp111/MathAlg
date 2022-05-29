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
    internal class EdmondsKarp
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\");
        }

        [Test]
        public void TestFluss1()
        {
            Assert.AreEqual("4.00", EdmondsKarpTest(@"flows\Fluss", 0, 7)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFluss2()
        {
            Assert.AreEqual("5.00", EdmondsKarpTest(@"flows\Fluss2", 0, 7)!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestG_1_2()
        {
            Assert.AreEqual("0.75447", EdmondsKarpTest(@"weighted\G_1_2", 0, 7)!.Value.ToString("0.00000", CultureInfo.InvariantCulture));
        }

        // Returns the length of the path or null if there is no path
        private double? EdmondsKarpTest(string fileName, int startID, int endID)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var file = $"{fileName}.txt";
                Graph graph = Graph.FromTextFile(file, true, true);
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var flow = graph.EdmondsKarp(startID, endID);
                Console.WriteLine($"Bellman-Ford: {flow}");
                stopwatch.Stop();
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
                var execTime = time - readTime;
                Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
                return flow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during BellmanFordTest: {ex}");
            }
            return 0;
        }
    }
}
