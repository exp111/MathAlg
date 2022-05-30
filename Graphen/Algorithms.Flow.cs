using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static double EdmondsKarp(this Graph graph, int startID, int endID)
        {
            double maxFlow = 0;
            // build residual graph network
            double[][] capacity = new double[graph.KnotenAnzahl][];
            for (var i = 0; i < graph.KnotenAnzahl; i++)
            {
                capacity[i] = new double[graph.KnotenAnzahl];
            }
            foreach (var edge in graph.Kanten)
            {
                capacity[edge.Start.ID][edge.Ende.ID] = edge.Capacity!.Value;
            }

            double[][] F = new double[graph.KnotenAnzahl][];
            for (var i = 0; i < graph.KnotenAnzahl; i++)
            {
                F[i] = new double[graph.KnotenAnzahl];
            }

            // bfs to 
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
                    // search neighbours (we need to search all cause we may need to use a backedge which isn't originally marked)
                    foreach (var other in graph.Knoten)
                    {
                        // dont add if the node was already added (and maybe even searched)
                        if (marked[other.ID])
                            continue;

                        // still residual capacity?
                        var residual = capacity[node.ID][other.ID] - F[node.ID][other.ID];
                        if (residual <= 0)
                            continue;

                        prev[other.ID] = node.ID;
                        // can only transfer as much as we can carry/carried till here
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
                // get new path
                var (flow, P) = Bfs();
                if (flow == 0) // if we dont find any path => stop
                    break;

                maxFlow += flow;
                var v = endID;
                var path = $"{v}";
                while (v != startID)
                {
                    var u = P[v];
                    path += $"->{u}";
                    F[u][v] += flow;
                    F[v][u] -= flow;
                    v = u;
                }
                Console.WriteLine($"round: flow: {maxFlow} ({flow}); path: {path}");
            }

            return maxFlow;
        }
    }
}
