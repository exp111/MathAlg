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
    internal class Kruskal
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraph15_18()
        {
            Assert.AreEqual("4.14000", KruskalTest("G_15_18").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph1k2k()
        {
            Assert.AreEqual("287.32286", KruskalTest("G_1_2").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph1k20k()
        {
            Assert.AreEqual("36.86275", KruskalTest("G_1_20").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph1k200k()
        {
            Assert.AreEqual("12.68182", KruskalTest("G_1_200").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph10k20k()
        {
            Assert.AreEqual("2785.62417", KruskalTest("G_10_20").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph10k200k()
        {
            Assert.AreEqual("372.14417", KruskalTest("G_10_200").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph100k200k()
        {
            Assert.AreEqual("27550.51488", KruskalTest("G_100_200").ToString("0.00000", CultureInfo.InvariantCulture));
        }

        private double KruskalTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFileWeighted($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var mst = graph.Kruskal();
                var count = mst.Kanten.GetWeight();
                Console.WriteLine($"Kruskal: {count}");
                stopwatch.Stop();
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
                var execTime = time - readTime;
                Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during KruskalTest: {ex}");
            }
            return 0;
        }
    }
}
