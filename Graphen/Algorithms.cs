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

        //TODO: use fibonacci heap for prio queue?
        //TODO: optimize
        public static Graph Prim(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            bool[] marked = new bool[graph.KnotenAnzahl];
            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> edges = new(edgeCount);

            var start = graph.Knoten[0];
            marked[start.ID] = true;
            foreach (var k in start.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            while (queue.Count > 0 && edges.Count < edgeCount)
            {
                var best = queue.Dequeue();
                // check if the other side was marked in the meantime
                if (marked[best.Start.ID] && marked[best.Ende.ID])
                    continue;

                // add to edge list
                edges.Add(best);
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

            // put it into a new graph
            return new Graph(graph.KnotenAnzahl, edges);
        }

        public static Graph PrimB(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            bool[] marked = new bool[graph.KnotenAnzahl];
            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> edges = new(edgeCount);

            var start = graph.Knoten[0];
            marked[start.ID] = true;
            foreach (var k in start.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            while (queue.Count > 0 && edges.Count < edgeCount)
            {
                var best = queue.Dequeue();
                // check if the other side was marked in the meantime
                if (marked[best.Start.ID] && marked[best.Ende.ID])
                    continue;

                // add to edge list
                edges.Add(best);
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

            // put it into a new graph
            return new Graph(graph.KnotenAnzahl, edges);
        }

        public class SubSet
        {
            public int Parent, Rank;
        }
        //TODO: optimize more? maybe path compression?
        public static Graph Kruskal(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            foreach (var k in graph.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> edges = new(edgeCount);
            // set set id (int) in array (access by node id)
            // save sets in to sets array (access by set id)
            SubSet[] nodes = new SubSet[graph.Knoten.Count];
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new SubSet() { Parent = i };
            }
            int find(int k)
            {
                var cur = k;
                while (nodes[cur].Parent != cur)
                    cur = nodes[cur].Parent;
                return cur;
            }

            void union(int nodeID, int otherID)
            {
                var node = nodes[nodeID];
                var other = nodes[otherID];

                // union-by-rank (make lower ranked parented to the higher rank)
                if (node.Rank < other.Rank)
                    node.Parent = otherID;
                else if (other.Rank > node.Rank)
                    other.Parent = nodeID;
                else // if both ranks are the same, make one higher rank
                {
                    other.Parent = nodeID;
                    node.Rank++;
                }
            }

            while (queue.Count > 0 && edges.Count < edgeCount)
            {
                var best = queue.Dequeue();

                var node = best.Start;
                var other = best.Ende;

                var nodeParent = find(node.ID);
                var otherParent = find(other.ID);
                // both sides are in the same set? already connected
                if (nodeParent == otherParent)
                    continue;

                // both sides are in different sets? merge
                union(nodeParent, otherParent);
                edges.Add(best);
            }

            // put it into a new graph
            return new Graph(graph.KnotenAnzahl, edges);
        }

        public static Graph KruskalB(this Graph graph)
        {
            PriorityQueue<Kante, double> queue = new();
            foreach (var k in graph.Kanten)
            {
                queue.Enqueue(k, k.Weight!.Value);
            }

            var edgeCount = graph.KnotenAnzahl - 1; // n - 1
            List<Kante> edges = new(edgeCount);
            // set set id (int) in array (access by node id)
            // save sets in to sets array (access by set id)
            SubSet[] nodes = new SubSet[graph.Knoten.Count];
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new SubSet() { Parent = i };
            }
            int find(int k)
            {
                var cur = k;
                while (nodes[cur].Parent != cur)
                    cur = nodes[cur].Parent;
                return cur;
            }

            void union(int nodeID, int otherID)
            {
                var node = nodes[nodeID];
                var other = nodes[otherID];

                // union-by-rank (make lower ranked parented to the higher rank)
                if (node.Rank < other.Rank)
                    node.Parent = otherID;
                else if (other.Rank > node.Rank)
                    other.Parent = nodeID;
                else // if both ranks are the same, make one higher rank
                {
                    other.Parent = nodeID;
                    node.Rank++;
                }
            }

            while (queue.Count > 0 && edges.Count < edgeCount)
            {
                var best = queue.Dequeue();

                var node = best.Start;
                var other = best.Ende;

                var nodeParent = find(node.ID);
                var otherParent = find(other.ID);
                // both sides are in the same set? already connected
                if (nodeParent == otherParent)
                    continue;

                // both sides are in different sets? merge
                union(nodeParent, otherParent);
                edges.Add(best);
            }

            // put it into a new graph
            return new Graph(graph.KnotenAnzahl, edges);
        }

        public static List<Kante> NearestNeighbour(this Graph graph)
        {
            List<Kante> edges = new();
            bool[] marked = new bool[graph.KnotenAnzahl];

            // select random node and mark it
            var start = graph.Knoten[0];
            var cur = start;
            marked[cur.ID] = true;
            // while not every node was searched (as we have a complete graph this means that
            // we will also have N-1 edges at that point (without the edge back to the start))
            while (edges.Count < graph.KnotenAnzahl - 1)
            {
                // choose the cheapest edge to a not marked node
                var cheapest = double.MaxValue;
                Kante? cheapestEdge = null;
                foreach (var edge in cur.Kanten)
                {
                    // get other end
                    var other = edge.Other(cur);
                    if (marked[other.ID]) // already marked
                        continue;

                    // check if the edge is cheaper
                    if (edge.Weight!.Value < cheapest)
                    {
                        cheapest = edge.Weight!.Value;
                        cheapestEdge = edge;
                    }
                }

                // select the other node as cur and mark it
                if (cheapestEdge != null)
                {
                    // add new edge to result
                    edges.Add(cheapestEdge);
                    // set cur
                    cur = cheapestEdge.Other(cur);
                    marked[cur.ID] = true;
                }
                else
                {
                    throw new Exception("NearestNeighbour didn't find a suitable edge. This shouldn't happen. Is your graph not complete?");
                }
            }
            // add edge from cur (last node) to start at the end
            //TODO: null handling mayhaps?
            edges.Add(cur.GetEdge(start)!);

            return edges;
        }

        public static List<Kante> DoubleTree(this Graph graph)
        {
            // first generate the mst
            var mst = graph.Prim();
            bool[] marked = new bool[mst.KnotenAnzahl];

            // run dfs and add nodes to list if not already added
            List<int> nodeIDs = new(mst.KnotenAnzahl); // NOTE: we collect the IDs because we search on the mst but connect later on the full graph
            Stack<Knoten> queue = new();

            var start = mst.Knoten[0];
            queue.Push(start);
            nodeIDs.Add(start.ID);
            // mark the start first (because we added it)
            marked[start.ID] = true;
            // add till we have all nodes
            while (queue.Count > 0 && nodeIDs.Count < mst.KnotenAnzahl)
            {
                var k = queue.Pop();
                // search neighbours
                foreach (var kante in k.Kanten)
                {
                    var other = kante.Other(k);
                    // dont add if the node was already added (and maybe even searched)
                    if (marked[other.ID])
                        continue;

                    // mark so we dont add twice
                    marked[other.ID] = true;
                    queue.Push(other);
                    nodeIDs.Add(other.ID);
                }
            }

            // now find the edges between the nodes we found (on the real graph
            List<Kante> edges = new(mst.KnotenAnzahl);
            for (var i = 0; i < nodeIDs.Count - 1; i++)
            {
                var cur = graph.Knoten[nodeIDs[i]];
                var next = graph.Knoten[nodeIDs[i + 1]];
                //TODO: null handling mayhaps?
                edges.Add(cur.GetEdge(next)!);
            }
            // also add edge back from the last to the first
            var first = graph.Knoten[nodeIDs[0]];
            var last = graph.Knoten[nodeIDs[nodeIDs.Count - 1]];
            edges.Add(last.GetEdge(first)!); //TODO: null handling :weary:

            return edges;
        }
    }
}
