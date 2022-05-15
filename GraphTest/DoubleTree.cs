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
    internal class DoubleTree
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\complete");
        }

        [Test]
        public void TestGraph10()
        {
            Assert.AreEqual("41.93", DoubleTreeTest("K_10").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph10e()
        {
            Assert.AreEqual("36.86", DoubleTreeTest("K_10e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12()
        {
            Assert.AreEqual("48.09", DoubleTreeTest("K_12").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12e()
        {
            Assert.AreEqual("40.09", DoubleTreeTest("K_12e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph15()
        {
            Assert.AreEqual("70.37", DoubleTreeTest("K_15").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph15e()
        {
            Assert.AreEqual("37.48", DoubleTreeTest("K_15e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph20()
        {
            Assert.AreEqual("99.69", DoubleTreeTest("K_20").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph30()
        {
            Assert.AreEqual("138.22", DoubleTreeTest("K_30").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph50()
        {
            Assert.AreEqual("230.94", DoubleTreeTest("K_50").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph100()
        {
            Assert.AreEqual("445.45", DoubleTreeTest("K_100").ToString("0.00", CultureInfo.InvariantCulture));
        }

        private double DoubleTreeTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var edges = graph.DoubleTree();
                var weight = edges.GetWeight();
                Console.WriteLine($"DoubleTree: {edges.GetPath()} ({weight})");
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
                Console.WriteLine($"Exception during DoubleTreeTest: {ex}");
            }
            return 0;
        }
    }
}
