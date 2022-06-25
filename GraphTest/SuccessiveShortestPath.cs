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
    internal class SuccessiveShortestPath
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\minimal_flows");
        }

        [Test]
        public void TestFluss1()
        {
            Assert.AreEqual("3.00", SuccessiveShortestPathTest("Kostenminimal1")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFluss2()
        {
            Assert.AreEqual("0.00", SuccessiveShortestPathTest("Kostenminimal2")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFluss3()
        {
            Assert.IsNull(SuccessiveShortestPathTest("Kostenminimal3"));
        }

        [Test]
        public void TestFluss4()
        {
            Assert.IsNull(SuccessiveShortestPathTest("Kostenminimal4"));
        }

        [Test]
        public void TestFlussGross1()
        {
            Assert.AreEqual("1537.00", SuccessiveShortestPathTest("Kostenminimal_gross1")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFlussGross2()
        {
            Assert.AreEqual("1838.00", SuccessiveShortestPathTest("Kostenminimal_gross2")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFlussGross3()
        {
            Assert.IsNull(SuccessiveShortestPathTest("Kostenminimal_gross3"));
        }

        // Returns the length of the path or null if there is no path
        private double? SuccessiveShortestPathTest(string fileName)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"Reading {fileName}");
                var file = $"{fileName}.txt";
                Graph graph = Graph.FromTextFileBalance(file, true);
                Console.WriteLine($"Read {fileName} ({graph.KnotenAnzahl} Knoten, {graph.KantenAnzahl} Kanten)");
                var readTime = stopwatch.Elapsed;
                var flow = graph.SuccessiveShortestPath();
                Console.WriteLine($"Successive Shortest Path: {(flow.HasValue ? flow : "No b-flow found")}");
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
                Console.WriteLine($"Exception during SuccessiveShortestPathTest: {ex}");
            }
            return 0;
        }
    }
}
