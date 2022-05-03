﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static List<Kante> Dijkstra(this Graph graph, int startID, int endID)
        {
            List<Knoten> queue = new();
            var dist = new double[graph.KnotenAnzahl];
            Array.Fill(dist, double.MaxValue); // dist = infinite //TODO: maybe null it instead?
            var start = graph.Knoten[startID];
            dist[start.ID] = 0;
            queue.Add(start);
            var pred = new int[graph.KnotenAnzahl];
            pred[start.ID] = start.ID;
            while (queue.Count > 0)
            {
                queue.Sort((v, w) =>
                {
                    return (int)(dist[v.ID] - dist[w.ID]);
                });
                var cur = queue[0];
                queue.RemoveAt(0);
                foreach (var edge in cur.Kanten)
                {
                    var newCost = dist[cur.ID] + edge.Weight!.Value;
                    var other = edge.Other(cur);
                    if (newCost < dist[other.ID])
                    {
                        dist[other.ID] = newCost;
                        pred[other.ID] = cur.ID;
                        queue.Add(other);
                    }
                }
            }

            //TODO: return graph
            return new();
        }

        public static bool BellmanFord(this Graph graph)
        {
            var start = graph.Knoten[0];
            var dist = new double[graph.KnotenAnzahl];
            Array.Fill(dist, double.MaxValue); // dist = infinite //TODO: maybe null it instead?
            dist[start.ID] = 0;
            var pred = new int[graph.KnotenAnzahl];
            pred[start.ID] = start.ID;

            // repeat n times
            for (var i = 0; i < graph.KnotenAnzahl - 1; i++)
            {
                //Console.WriteLine($"iteration: {i + 1}");
                foreach (var edge in graph.Kanten)
                {
                    var v = edge.Start.ID;
                    var w = edge.Ende.ID;
                    var c = edge.Weight!.Value;
                    var newCost = dist[v] + c;
                    if (newCost < dist[w])
                    {
                        //Console.WriteLine($"changing {w+1} cause {dist[v]}+{c} < {dist[w]}");
                        dist[w] = newCost;
                        pred[w] = v;
                    }
                }
            }


            //TODO: return graph
            return false;
        }
    }
}
