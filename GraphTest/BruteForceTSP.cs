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
    internal class BranchBoundTSP
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data");
        }

        [Test]
        public void TestGraph10()
        {
            Assert.AreEqual("38.41", BranchBoundTest("K_10").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph10e()
        {
            Assert.AreEqual("27.26", BranchBoundTest("K_10e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12()
        {
            Assert.AreEqual("45.19", BranchBoundTest("K_12").ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestGraph12e()
        {
            Assert.AreEqual("36.13", BranchBoundTest("K_12e").ToString("0.00", CultureInfo.InvariantCulture));
        }

        // Any more tests are unrealistic

        private double BranchBoundTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var graph = Graph.FromTextFile($"{fileName}.txt");
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var edges = graph.BranchBoundTSP();
                var weight = edges.GetWeight();
                Console.WriteLine($"Bruteforce: {edges.GetPath()} ({weight})");
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
