using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static bool CalculateBFlow(Graph graph, double[][] F)
        {
            // flow from the super source/sink
            var sourceFlow = 0d;
            var sinkFlow = 0d;

            // create supersource + sink and add them to the graph
            var source = new Knoten(graph.KnotenAnzahl, 0, 0);
            graph.AddNode(source);
            var sink = new Knoten(graph.KnotenAnzahl, 0, 0);
            graph.AddNode(sink);

            // link supersource to sources, sinks to supersink
            foreach (var node in graph.Knoten)
            {
                var b = node.Balance!.Value;
                if (b == 0)
                    continue;

                if (b > 0)
                {
                    graph.AddKante(new(source, node, 0, b, true));
                    sourceFlow += b;
                }
                else
                {
                    graph.AddKante(new(node, sink, 0, b * -1, true));
                    sinkFlow += b * -1;
                }
            }

            // now run karp to find a flow that fulfills all balances (a b flow)
            var flow = graph.EdmondsKarp(source.ID, sink.ID, F);
            // if the maximum flow is not equal the max out of the source/into the sink, we haven't fulfilled the balance
            return flow == sourceFlow && flow == sinkFlow; //TODO: probably do some epsilon shit
        }

        // Creates a residual graph from the graph and the 
        public static Graph CreateResidualGraph(Graph graph, double[][] F)
        {
            // Create new graph
            var residual = new Graph(graph.KnotenAnzahl);

            // Copy balance over
            foreach (var node in graph.Knoten)
                residual.Knoten[node.ID].Balance = node.Balance;

            foreach (var edge in graph.Kanten)
            {
                var fromID = edge.Start.ID;
                var toID = edge.Ende.ID;
                var from = residual.Knoten[fromID]; // the new nodes
                var to = residual.Knoten[toID];
                var weight = edge.Weight;
                var capacity = edge.Capacity;
                var flow = F[fromID][toID];
                var remainder = capacity - flow;
                // if there is no remainder, only create a backedge
                if (remainder == 0)
                {
                    residual.AddKante(new Kante(to, from, weight * -1, 0, true));
                    continue;
                }
                // if we havent used it yet, add the same edge
                else if (flow == 0)
                {
                    residual.AddKante(new Kante(from, to, weight, capacity, true));
                    continue;
                }

                // else create to and backedges (with negative weight for the backedge)
                residual.AddKante(new Kante(from, to, weight, capacity, true));
                residual.AddKante(new Kante(to, from, weight * -1, 0, true));
            }
            return residual;
        }

        // Returns a negative cycle or null if none exists
        public static List<Kante>? GetNegativeCycle(Graph graph)
        {
            // create new super node to search from
            var node = new Knoten(graph.KnotenAnzahl, 0, 0);
            graph.AddNode(node);
            foreach (var n in graph.Knoten)
            {
                graph.AddKante(new Kante(node, n, 0, 0, true));
            }
            //note: we don't need to delete this since we throw the graph away anyways

            var (tree, edge) = graph.BellmanFord(node.ID);
            if (edge == null) // no cycle found
                return null;

            return tree!.GetNegativeCycle(edge);
        }

        /* Calculates the minimum cost flow
         * Returns null if a b flow couldn't be created else the minimum 
         */
        public static double? CycleCanceling(this Graph graph)
        {
            // the current used flow
            double[][] F = new double[graph.KnotenAnzahl + 2][]; //+2 cuz we need to add the supersink + src
            for (var i = 0; i < graph.KnotenAnzahl + 2; i++)
            {
                F[i] = new double[graph.KnotenAnzahl + 2];
            }

            // Step 1: calculate b flow
            var exists = CalculateBFlow(graph, F);
            if (!exists)
                return null;

            while (true)
            {
                // step 2: create residual graph
                var residual = CreateResidualGraph(graph, F);
                // step 3: find negative cycle
                var cycle = GetNegativeCycle(residual);
                if (cycle == null) // if none exist, we're minimal => exit
                    break;
                // get minimum remainder (cap - flow) from cycle
                var min = cycle.Min(e => e.Capacity - F[e.Start.ID][e.Ende.ID])!.Value;
                // step 4: augment flow along the cycle by minimum remainder
                foreach (var edge in cycle)
                {
                    var f = edge.Start.ID;
                    var t = edge.Ende.ID;
                    F[f][t] += min;
                    F[t][f] -= min;
                }
            }

            // calculate cost
            var cost = 0d;
            foreach (var edge in graph.Kanten)
            {
                cost += edge.Weight!.Value * F[edge.Start.ID][edge.Ende.ID];
            }

            return cost;
        }

        /* Calculates the minimum cost flow
         * Returns null if a b flow couldn't be created else the minimum 
         */
        public static double? SuccessiveShortestPath(this Graph graph)
        {
            // the current used flow
            double[][] F = new double[graph.KnotenAnzahl][];
            for (var i = 0; i < graph.KnotenAnzahl; i++)
            {
                F[i] = new double[graph.KnotenAnzahl];
            }
            // b'
            double[] Bs = new double[graph.KnotenAnzahl];
            // step 1: set flow to capacity if edge has negative weight
            foreach (var edge in graph.Kanten)
            {
                if (edge.Weight < 0)
                {
                    var from = edge.Start.ID;
                    var to = edge.Ende.ID;
                    F[from][to] = edge.Capacity!.Value;
                    F[to][from] = edge.Capacity!.Value * -1;
                }
            }
            
            //TODO: initialize Bs here
            while (true)
            {
                var residual = CreateResidualGraph(graph, F);
                
                // step 2: select connected nodes s with b - b' > 0 and t with b - b' < 0 (can still be improved)
                // find suitable nodes
                var srcCanidates = new List<Knoten>();
                var dstCanidates = new List<Knoten>();
                foreach (var node in graph.Knoten)
                {
                    // calculate b'
                    var b = 0d;
                    // check flows from/to node and add/subtract them to balance
                    foreach (var flow in F[node.ID])
                        b += flow;

                    Bs[node.ID] = b;
                    if (node.Balance > b) // b - b' > 0 => potential s
                        srcCanidates.Add(node);
                    else if (node.Balance < b) // b - b' < 0 => potential t
                        dstCanidates.Add(node);
                }
                // find s and t pair that is connected
                bool PathExists(int startID, int endID)
                {
                    // uses bfs to find a path
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

                            // if a edge exists, use that capacity, otherwise use 0 (backedges have a cap of 0, so it works)
                            var edge = node.GetEdge(other);
                            var capacity = edge is not null ? edge.Capacity : 0;

                            // still residual capacity? (capacity - used flow)
                            var residual = capacity - F[node.ID][other.ID];
                            if (residual <= 0)
                                continue;

                            if (other.ID == endID)
                                return true;

                            // mark so we dont add twice
                            marked[other.ID] = true;
                            queue.Enqueue(other);
                        }
                    }
                    return false;
                }

                Knoten? s = null, t = null;
                //TODO: always take first non blacklisted src and blacklist it if we cant find sink
                foreach (var src in srcCanidates)
                {
                    //TODO: look which nodes are reachable from src, then check if dsts is under them

                    foreach (var dst in dstCanidates)
                    {
                        // check if reachable with a bfs => valid pair
                        if (PathExists(src.ID, dst.ID))
                        {
                            s = src;
                            t = dst;
                            goto found;
                        }
                    }
                }
            found:
                // if no such pair is found, we're either minimal or no b-flow can be created
                if (s is null || t is null) 
                    break;


                // step 3: calc shortest path between s and t in residual
                var (tree, cycleEdge) = residual.BellmanFord(s.ID);
                if (cycleEdge != null) // can we ignore this?
                    throw new Exception("Found negative cycle!");
                var path = tree.GetShortestPath(t.ID);
                if (path == null)
                    throw new Exception("Couldn't find a shortest path (impossible challenge)!");
                // get minimum from
                var min = path!.Min(e => e.Capacity - F[e.Start.ID][e.Ende.ID])!.Value;
                min = Math.Min(min, // remainder on path
                    Math.Min(s.Balance!.Value - Bs[s.ID], // b(s) - b'(s)
                        Math.Abs(Bs[t.ID] - t.Balance!.Value))); // b'(t) - b(t) //TODO: do we need the abs

                // step 4: update flow along the path
                foreach (var edge in path)
                {
                    var from = edge.Start.ID;
                    var to = edge.Ende.ID;
                    F[from][to] += min;
                    F[to][from] -= min;
                }
                //TODO: update Bs here instead of every loop (s + t)
            }

            // check if the flow is minimal (by checking if b == b' for all nodes)
            foreach (var node in graph.Knoten)
            {
                if (node.Balance != Bs[node.ID])
                    return null; // not minimal => no b-flow
            }

            // calculate cost
            var cost = 0d;
            foreach (var edge in graph.Kanten)
            {
                cost += edge.Weight!.Value * F[edge.Start.ID][edge.Ende.ID];
            }

            return cost;
        }
    }
}
