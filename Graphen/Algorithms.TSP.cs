using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
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
                var node = queue.Pop();
                // search neighbours
                foreach (var edge in node.Kanten)
                {
                    var other = edge.Other(node);
                    // dont add if the node was already added (and maybe even searched)
                    if (marked[other.ID])
                        continue;

                    // mark so we dont add twice
                    marked[other.ID] = true;
                    queue.Push(other);
                    nodeIDs.Add(other.ID);
                }
            }

            // now find the edges between the nodes we found (on the real graph)
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

        public static List<Kante> BruteForceTSP(this Graph graph)
        {
            return null;
        }

        public static List<Kante> BranchBoundTSP(this Graph graph)
        {
            return null;
        }
    }
}
