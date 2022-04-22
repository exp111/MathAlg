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

        //TODO: optimize, return MST class?
        public static double Kruskal(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            foreach (var k in graph.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> kanten = new(edgeCount);
            // set set id (int) in array (access by node id)
            // save sets in to sets array (access by set id)
            int[] nodes = new int[graph.Knoten.Count];
            Dictionary<int, List<int>?> sets = new();
            var setCounter = 1;

            while (queue.Count > 0 && kanten.Count < edgeCount)
            {
                var best = queue.Dequeue();

                var node = best.Start;
                var other = best.Ende;

                // is it unmarked?
                if (nodes[node.ID] != 0)
                {
                    // first one not unmarked. work with the other one
                    node = best.Ende;
                    other = best.Start;
                }
                else
                {
                    // both are empty => make new set
                    if (nodes[other.ID] == 0)
                    {
                        nodes[node.ID] = setCounter;
                        nodes[other.ID] = setCounter;
                        sets[setCounter] = new List<int> { node.ID, other.ID };
                        setCounter++;
                        kanten.Add(best);
                        continue; // we did our job
                    }
                }

                // both sides are in the same set? already connected
                if (nodes[best.Start.ID] == nodes[best.Ende.ID])
                    continue;

                // only this one unmarked? => join into other set
                if (nodes[node.ID] == 0)
                {
                    var otherSetID = nodes[other.ID];
                    // add to set
                    nodes[node.ID] = otherSetID;
                    sets[otherSetID]!.Add(node.ID);
                    // we were successful
                    kanten.Add(best);
                    continue;
                }

                // both sides are in different sets? merge
                var mergerSetID = nodes[node.ID];
                var mergeeSetID = nodes[other.ID];
                var mergeeSet = sets[mergeeSetID]!;

                sets[mergerSetID]!.AddRange(mergeeSet);
                foreach (var mergee in mergeeSet)
                {
                    nodes[mergee] = mergerSetID;
                }
                sets[mergeeSetID] = null;
                kanten.Add(best);
            }

            var count = 0.0d;
            foreach (var k in kanten)
                count += k.Weight!.Value;

            return count;
        }
    }
}
