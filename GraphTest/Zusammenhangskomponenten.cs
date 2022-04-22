using Graphen;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace GraphTest
{
    public class Zusammenhangskomponenten
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraphSmall()
        {
            Assert.AreEqual(2, RunZusammenhangskomponentenTest("Graph1"));
        }

        [Test]
        public void TestGraphMedium()
        {
            Assert.AreEqual(4, RunZusammenhangskomponentenTest("Graph2"));
        }

        [Test]
        public void TestGraphMedium2()
        {
            Assert.AreEqual(4, RunZusammenhangskomponentenTest("Graph3"));
        }

        [Test]
        public void TestGraphBig()
        {
            Assert.AreEqual(222, RunZusammenhangskomponentenTest("Graph_gross"));
        }

        [Test]
        public void TestGraphBig2()
        {
            Assert.AreEqual(9560, RunZusammenhangskomponentenTest("Graph_ganzgross"));
        }

        [Test]
        public void TestGraphBig3()
        {
            Assert.AreEqual(306, RunZusammenhangskomponentenTest("Graph_ganzganzgross"));
        }

        private int RunZusammenhangskomponentenTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var count = graph.GetZusammenhangskomponenten();
                Console.WriteLine($"Zusammenhangskomponenten: {count}");
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