using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static ShortestPathTree Dijkstra(this Graph graph, int startID)
        {
            List<Knoten> queue = new(); //TODO: use a prio instead of sorting?
            var marked = new bool[graph.KnotenAnzahl];
            var dist = new double[graph.KnotenAnzahl];
            var pred = new int[graph.KnotenAnzahl];
            Array.Fill(dist, double.PositiveInfinity); // dist = infinite
            Array.Fill(pred, Knoten.InvalidID); // pred = NULL

            var start = graph.Knoten[startID];
            dist[start.ID] = 0;
            pred[start.ID] = start.ID;
            queue.Add(start);
            
            while (queue.Count > 0)
            {
                queue.Sort((v, w) =>
                {
                    return (int)(dist[v.ID] - dist[w.ID]);
                });
                var cur = queue[0];
                queue.RemoveAt(0);
                // if the node got relaxed in the meanwhile, dont add it
                if (marked[cur.ID])
                    continue;

                // mark this one
                marked[cur.ID] = true;
                // then check other neighbours
                foreach (var edge in cur.Kanten)
                {
                    // dont check the other node if its already relaxed
                    var other = edge.Other(cur);
                    if (marked[other.ID])
                        continue;

                    // check if we got a better cost
                    var newCost = dist[cur.ID] + edge.Weight!.Value;
                    
                    if (newCost < dist[other.ID])
                    {
                        dist[other.ID] = newCost;
                        pred[other.ID] = cur.ID;
                        queue.Add(other); //TODO: this currently can add a node v d(v) times. 
                        // marking the node here would remove that, but would make us check the distance even if the node is fully relaxed
                        // other idea: new "added" array, more memory 
                        //TODO: benchmark current thing, marking only here, and with an added arr
                    }
                }
            }

            return new(graph, dist, pred);
        }

        // Returns the shortest path tree and a edge inside a negative cycle, if one exists
        public static (ShortestPathTree, Kante?) BellmanFord(this Graph graph, int startID = 0)
        {
            var dist = new double[graph.KnotenAnzahl];
            var pred = new int[graph.KnotenAnzahl];
            Array.Fill(dist, double.PositiveInfinity); // dist = infinite
            Array.Fill(pred, Knoten.InvalidID); // pred = NULL

            var start = graph.Knoten[startID];
            dist[start.ID] = 0;
            pred[start.ID] = start.ID;

            var changed = true;
            // repeat n - 1 times
            for (var i = 0; i < graph.KnotenAnzahl - 1; i++)
            {
                // check if we changed anything last iteration
                if (!changed)
                    break; // if not no need to run any longer

                changed = false;
                foreach (var edge in graph.Kanten)
                {
                    var v = edge.Start.ID;
                    var w = edge.Ende.ID;
                    var c = edge.Weight!.Value;
                    var newCost = dist[v] + c;
                    if (newCost < dist[w])
                    {
                        dist[w] = newCost;
                        pred[w] = v;
                        changed = true;
                    }

                    // if its undirected, also check the other side
                    // we need to do this scuffed move cause we dont create double edges
                    //TODO: instead create undirected graphs differently?
                    if (!edge.Directed)
                    {
                        newCost = dist[w] + c;
                        if (newCost < dist[v])
                        {
                            dist[v] = newCost;
                            pred[v] = w;
                        }
                    }
                }
            }

            // in the n-th iteration check if the distance still changes
            foreach (var edge in graph.Kanten)
            {
                // if it would, we got a negative cycle
                if (dist[edge.Start.ID] + edge.Weight!.Value < dist[edge.Ende.ID])
                    return (new(graph, dist, pred), edge);
                //note: what we return here isnt a valid shortest tree path technically, but we need the pred array
            }


            return (new(graph, dist, pred), null);
        }
    }
}
