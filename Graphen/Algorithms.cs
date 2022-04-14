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
                Dictionary<int, bool> marked = new();

                // marks all nodes from the start node
                void Bfs(Knoten start)
                {
                    Queue<Knoten> queue = new();
                    queue.Enqueue(start);
                    while (queue.Count > 0)
                    {
                        var k = queue.Dequeue();
                        // already marked beforehand? 
                        if (marked.ContainsKey(k.ID))
                            continue;

                        // mark and search
                        marked[k.ID] = true;
                        foreach (var kante in k.Kanten)
                        {
                            var other = kante.Start.ID == k.ID ? kante.Ende : kante.Start;
                            if (marked.ContainsKey(other.ID))
                                continue;

                            queue.Enqueue(other);
                        }
                    }
                }

                // look through all nodes
                var count = 0;
                foreach (var knoten in graph.Knoten)
                {
                    // if its marked we already discovered it
                    if (marked.ContainsKey(knoten.ID))
                        continue;

                    // else we have found a new sub graph, then mark all nodes in that graph
                    Debug.WriteLine($"New sub graph from: {knoten}");
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
