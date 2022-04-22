using Graphen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTest
{
    internal class Prim
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraph15_18()
        {
            Assert.AreEqual(4.140000000000001, PrimTest("G_15_18"));
        }

        [Test]
        public void TestGraph1k2k()
        {
            Assert.AreEqual(287.32286000000011, PrimTest("G_1_2"));
        }

        [Test]
        public void TestGraph1k20k()
        {
            Assert.AreEqual(36.862749999999991, PrimTest("G_1_20"));
        }

        [Test]
        public void TestGraph1k200k()
        {
            Assert.AreEqual(12.681820000000016, PrimTest("G_1_200"));
        }

        [Test]
        public void TestGraph10k20k()
        {
            Assert.AreEqual(2785.6241700000019, PrimTest("G_10_20"));
        }

        [Test]
        public void TestGraph10k200k()
        {
            Assert.AreEqual(372.14416999999963, PrimTest("G_10_200"));
        }

        [Test]
        public void TestGraph100k200k()
        {
            Assert.AreEqual(27550.51488000021, PrimTest("G_100_200"));
        }

        private double PrimTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFileWeighted($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var count = graph.Prim();
                Console.WriteLine($"Prim: {count}");
                stopwatch.Stop();
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{fileName} took {(int)time.TotalMilliseconds} ms ({(int)time.TotalSeconds} seconds)");
                Console.WriteLine($"Read Time: {(int)readTime.TotalMilliseconds} ms ({(int)readTime.TotalSeconds} seconds)");
                var execTime = time - readTime;
                Console.WriteLine($"Exec Time: {(int)execTime.TotalMilliseconds} ms ({(int)execTime.TotalSeconds} seconds)");
                Console.WriteLine();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during RunZusammenhangkomponentenTest: {ex}");
            }
            return 0;
        }
    }
}
