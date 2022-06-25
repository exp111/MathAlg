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
    internal class CycleCanceling
    {
        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(@"E:\D\Visual Studio\Uni\MathAlg\Graphen\data\minimal_flows");
        }

        [Test]
        public void TestFluss1()
        {
            Assert.AreEqual("3.00", CycleCancelingTest("Kostenminimal1")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFluss2()
        {
            Assert.AreEqual("0.00", CycleCancelingTest("Kostenminimal2")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFluss3()
        {
            Assert.IsNull(CycleCancelingTest("Kostenminimal3"));
        }

        [Test]
        public void TestFluss4()
        {
            Assert.IsNull(CycleCancelingTest("Kostenminimal4"));
        }

        [Test]
        public void TestFlussGross1()
        {
            Assert.AreEqual("1537.00", CycleCancelingTest("Kostenminimal_gross1")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFlussGross2()
        {
            Assert.AreEqual("1838.00", CycleCancelingTest("Kostenminimal_gross2")!.Value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestFlussGross3()
        {
            Assert.IsNull(CycleCancelingTest("Kostenminimal_gross3"));
        }

        // Returns the length of the path or null if there is no path
        private double? CycleCancelingTest(string fileName)
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
                var flow = graph.CycleCanceling();
                Console.WriteLine($"Cycle Canceling: {flow}");
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
                Console.WriteLine($"Exception during CycleCancelingTest: {ex}");
            }
            return 0;
        }
    }
}
