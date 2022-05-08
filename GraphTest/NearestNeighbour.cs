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
    internal class NearestNeighbour
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraph10()
        {
            Assert.AreEqual("41.17", NearestNeighbourTest("K_10").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph10e()
        {
            Assert.AreEqual("32.42", NearestNeighbourTest("K_10e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12()
        {
            Assert.AreEqual("50.43", NearestNeighbourTest("K_12").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12e()
        {
            Assert.AreEqual("37.63", NearestNeighbourTest("K_12e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph15()
        {
            Assert.AreEqual("53.21", NearestNeighbourTest("K_15").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph15e()
        {
            Assert.AreEqual("38.16", NearestNeighbourTest("K_15e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph20()
        {
            Assert.AreEqual("74.83", NearestNeighbourTest("K_20").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph30()
        {
            Assert.AreEqual("105.67", NearestNeighbourTest("K_30").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph50()
        {
            Assert.AreEqual("171.28", NearestNeighbourTest("K_50").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph100()
        {
            Assert.AreEqual("323.93", NearestNeighbourTest("K_100").ToString("0.00", CultureInfo.InvariantCulture));
        }

        private double NearestNeighbourTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var edges = graph.NearestNeighbour();
                var weight = edges.GetWeight();
                Console.WriteLine($"NearestNeighbour: {edges.GetPath()} ({weight})");
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
                Console.WriteLine($"Exception during NearestNeighbourTest: {ex}");
            }
            return 0;
        }
    }
}
