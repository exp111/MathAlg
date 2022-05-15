using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static List<Kante> Dijkstra(this Graph graph, int startID)
        {
            List<Knoten> queue = new();
            var dist = new double[graph.KnotenAnzahl];
            Array.Fill(dist, double.PositiveInfinity); // dist = infinite
            var start = graph.Knoten[startID];
            dist[start.ID] = 0;
            queue.Add(start);
            var pred = new int[graph.KnotenAnzahl];
            Array.Fill(pred, -1); // pred = NULL // TODO: maybe do null instead?
            pred[start.ID] = start.ID;
            //TODO: marked
            while (queue.Count > 0)
            {
                queue.Sort((v, w) =>
                {
                    return (int)(dist[v.ID] - dist[w.ID]);
                });
                var cur = queue[0];
                queue.RemoveAt(0);
                foreach (var edge in cur.Kanten)
                {
                    var newCost = dist[cur.ID] + edge.Weight!.Value;
                    var other = edge.Other(cur);
                    if (newCost < dist[other.ID])
                    {
                        dist[other.ID] = newCost;
                        pred[other.ID] = cur.ID;
                        queue.Add(other);
                    }
                }
            }

            //TODO: return graph/shortest path tree/predecessor arr
            return new();
        }

        // returns the shortest way graph and if the graph contains a negative cycle
        public static (Graph, bool) BellmanFord(this Graph graph, int startID = 0)
        {
            var start = graph.Knoten[startID];
            var dist = new double[graph.KnotenAnzahl];
            Array.Fill(dist, double.PositiveInfinity); // dist = infinite //TODO: maybe null it instead?
            dist[start.ID] = 0;
            var pred = new int[graph.KnotenAnzahl];
            Array.Fill(pred, -1); // pred = NULL // TODO: maybe do null instead?
            pred[start.ID] = start.ID;

            // repeat n - 1 times
            for (var i = 0; i < graph.KnotenAnzahl - 1; i++)
            {
                //TODO: if (!changed) break
                //TODO: changed = false;
                //Console.WriteLine($"iteration: {i + 1}");
                foreach (var edge in graph.Kanten)
                {
                    var v = edge.Start.ID;
                    var w = edge.Ende.ID;
                    var c = edge.Weight!.Value;
                    var newCost = dist[v] + c;
                    if (newCost < dist[w])
                    {
                        //Console.WriteLine($"changing {w+1} cause {dist[v]}+{c} < {dist[w]}");
                        dist[w] = newCost;
                        pred[w] = v;
                        //TODO: changed = true;
                    }
                }
            }


            //TODO: return graph
            // what should we return here?
            // the graph, but then the shortest path search would be more inefficient.
            // or the pred list? but that would then require a getEdge again.

            //TODO: return a "handle to the cycle"; meaning the edge where we found it?
            return (new(0), false);
        }
    }
}
