using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static class Algorithms
    {
        public static int GetZusammenhangskomponenten(this Graph graph)
        {
            try
            {
                // so we can remember which nodes are already marked. access via node id
                bool[] marked = new bool[graph.KnotenAnzahl];

                // marks all nodes from the start node
                void Bfs(Knoten start)
                {
                    Queue<Knoten> queue = new();
                    queue.Enqueue(start);
                    // mark the start first (because we added it)
                    marked[start.ID] = true;
                    while (queue.Count > 0)
                    {
                        var k = queue.Dequeue();
                        // search neighbours
                        foreach (var kante in k.Kanten)
                        {
                            var other = kante.Other(k);
                            // dont add if the node was already added (and maybe even searched)
                            if (marked[other.ID])
                                continue;

                            // mark so we dont add twice
                            marked[other.ID] = true;
                            queue.Enqueue(other);
                        }
                    }
                }

                // look through all nodes
                var count = 0;
                foreach (var knoten in graph.Knoten)
                {
                    // if its marked we already discovered it
                    if (marked[knoten.ID])
                        continue;

                    // else we have found a new sub graph, then mark all nodes in that graph
                    count++;
                    Bfs(knoten);
                }

                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Algorithms.GetZusammenhangskomponenten: {ex}");
            }
            return 0;
        }

        //TODO: optimize, return MST class?
        public static double Prim(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            bool[] marked = new bool[graph.Knoten.Count];
            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> kanten = new(edgeCount);

            var start = graph.Knoten[0];
            marked[start.ID] = true;
            foreach (var k in start.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            while (queue.Count > 0 && kanten.Count < edgeCount)
            {
                var best = queue.Dequeue();
                // check if the other side was marked in the meantime
                if (marked[best.Start.ID] && marked[best.Ende.ID])
                    continue;

                // add to edge list
                kanten.Add(best);
                // get node we havent checked
                var other = best.Start;
                if (marked[other.ID])
                    other = best.Ende;

                // mark and add other edges to the queue
                marked[other.ID] = true;
                foreach (var k in other.Kanten)
                {
                    // check if node on the other end is already marked so we dont get a circle
                    if (marked[k.Other(other).ID])
                        continue;

                    queue.Enqueue(k, k.Weight!.Value);
                }
            }

            var count = 0.0d;
            foreach (var k in kanten)
                count += k.Weight!.Value;

            return count;
        }

        //TODO: kruskal, return MST class?
        public static int Kruskal(this Graph graph)
        {
            // set set id (int) in array (access by node id)
            // save sets in to sets array (access by set id)
            return 0;
        }
    }
}
