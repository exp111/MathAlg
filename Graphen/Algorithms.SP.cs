using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static bool BellmanFord(this Graph graph)
        {
            var start = graph.Knoten[0];
            var dist = new double[graph.KnotenAnzahl];
            Array.Fill(dist, double.MaxValue); // dist = infinite //TODO: maybe null it instead?
            dist[start.ID] = 0;
            var pred = new int[graph.KnotenAnzahl];
            pred[start.ID] = start.ID;

            // repeat n times
            for (var i = 0; i < graph.KnotenAnzahl - 1; i++)
            {
                Console.WriteLine($"iteration: {i + 1}");
                foreach (var edge in graph.Kanten)
                {
                    var v = edge.Start.ID;
                    var w = edge.Ende.ID;
                    var c = edge.Weight!.Value;
                    if (dist[v] + c < dist[w])
                    {
                        Console.WriteLine($"changing {w+1} cause {dist[v]}+{c} < {dist[w]}");
                        dist[w] = dist[v] + c; //todo: optimize by temp saving
                        pred[w] = v;
                    }
                }
            }


            //TODO: return graph
            return false;
        }
    }
}
