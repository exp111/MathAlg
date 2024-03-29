﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
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
    }
}
