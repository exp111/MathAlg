using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static double EdmondsKarp(this Graph graph, int startID, int endID, double[][] F)
        {
            double maxFlow = 0;
            // build capacity of the graph network
            double[][] capacity = new double[graph.KnotenAnzahl][];
            for (var i = 0; i < graph.KnotenAnzahl; i++)
            {
                capacity[i] = new double[graph.KnotenAnzahl];
            }
            foreach (var edge in graph.Kanten)
            {
                capacity[edge.Start.ID][edge.Ende.ID] = edge.Capacity!.Value;
            }
            // the other values are 0 (backedges have a cap of 0)

            // bfs to find the shortest path
            (double, int[]) Bfs()
            {
                bool[] marked = new bool[graph.KnotenAnzahl];
                double[] cost = new double[graph.KnotenAnzahl];
                int[] prev = new int[graph.KnotenAnzahl];
                Queue<Knoten> queue = new();
                queue.Enqueue(graph.Knoten[startID]);
                // mark the start first (because we added it)
                marked[startID] = true;
                // set cost to inf
                cost[startID] = double.PositiveInfinity;
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();
                    // search neighbours (we need to search all cause we may need to use a backedge which isn't originally connected)
                    foreach (var other in graph.Knoten)
                    {
                        // dont add if the node was already added (and maybe even searched)
                        if (marked[other.ID])
                            continue;

                        // still residual capacity? (capacity - used flow)
                        var residual = capacity[node.ID][other.ID] - F[node.ID][other.ID];
                        if (residual <= 0)
                            continue;

                        prev[other.ID] = node.ID;
                        // can only transfer as much as we can carry (residual)/carried till here (cost[v])
                        cost[other.ID] = Math.Min(cost[node.ID], residual);

                        if (other.ID == endID)
                            return (cost[other.ID], prev);

                        // mark so we dont add twice
                        marked[other.ID] = true;
                        queue.Enqueue(other);
                    }
                }
                return (0, Array.Empty<int>());
            }

            while (true)
            {
                // get new path (bfs to get the shortest flow for a lower chance of getting a bottleneck)
                var (flow, P) = Bfs();
                if (flow == 0) // if we dont find any path => stop
                    break;

                // add this flow to the maxflow
                maxFlow += flow;
                // augment the edges on the path
                var v = endID;
                while (v != startID)
                {
                    var u = P[v];
                    F[u][v] += flow; // add to the edge that we take
                    F[v][u] -= flow; // subtract from the backedge
                    v = u;
                }
            }

            return maxFlow;
        }
        public static double EdmondsKarp(this Graph graph, int startID, int endID)
        {
            // the current used flow
            double[][] F = new double[graph.KnotenAnzahl][];
            for (var i = 0; i < graph.KnotenAnzahl; i++)
            {
                F[i] = new double[graph.KnotenAnzahl];
            }
            return EdmondsKarp(graph, startID, endID, F);
        }
    }
}
