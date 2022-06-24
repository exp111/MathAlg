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
            var sourceFlow = 0d;
            var sinkFlow = 0d;

            List<Knoten> nodes = new();

            // calculate the source balance from every source (and sink balance from every sink)
            foreach (var node in graph.Knoten)
            {
                var b = node.Balance!.Value;
                if (b == 0)
                    continue;

                if (b > 0)
                    sourceFlow += b;
                else
                    sinkFlow += b;
                nodes.Add(node);
            }

            // create supersource + sink and add them to the graph
            var source = new Knoten(graph.KnotenAnzahl, 0, sourceFlow);
            graph.AddNode(source);
            var sink = new Knoten(graph.KnotenAnzahl, 0, sinkFlow);
            graph.AddNode(sink);

            // link supersource to sources, sinks to supersink
            foreach (var node in nodes)
            {
                var b = node.Balance!.Value;
                if (b > 0)
                    graph.AddKante(new(source, node, 0, b, true));
                else
                    graph.AddKante(new(node, sink, 0, b * -1, true));
            }

            // now run karp //TODO: why?
            var flow = graph.EdmondsKarp(source.ID, sink.ID, F);
            return flow == sourceFlow;
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
            
            var (tree, edge) = graph.BellmanFord();
            if (edge == null) // no cycle found
                return null;

            return tree!.GetNegativeCycle(edge);
        }     

        // Calculates the minimum cost flow
        /*
         * Returns NaN if a b flow couldn't be created else the minimum 
         */
        public static double CycleCanceling(this Graph graph)
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
                return double.NaN;

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
        //TODO: tests

        public static Graph SuccessiveShorestPath(this Graph graph)
        {
            return new Graph(0);
        }
    }
}
