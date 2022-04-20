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
                            var other = kante.Start.ID == k.ID ? kante.Ende : kante.Start;
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
    }
}
