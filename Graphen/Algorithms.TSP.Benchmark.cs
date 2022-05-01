using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static List<Kante> BruteForceTSPB(this Graph graph)
        {
            //TODO: maybe only give start ID
            //TODO: maybe move reffed items like marked, bestTourCost, bestTour into whole func scope
            var marked = new bool[graph.KnotenAnzahl];
            var bestTourCost = double.MaxValue;
            var bestTour = new Kante[graph.KnotenAnzahl];
            var tour = new Kante[graph.KnotenAnzahl];

            // generates all circle tour permutations (in a complete graph) and saves the best one in bestTour
            void permute(int lvl, Knoten start, Knoten cur, double tourCost)
            {
                // mark the current node
                marked[cur.ID] = true;
                // finished this tour?
                if (lvl == graph.KnotenAnzahl - 1)
                {
                    var returnEdge = cur.GetEdge(start)!;
                    var newCost = tourCost + returnEdge.Weight!.Value;
                    // check if the cost is lower than the previous routes => save
                    if (newCost < bestTourCost)
                    {
                        bestTourCost = newCost;
                        tour[lvl] = returnEdge; // dont forget the edge back
                        tour.CopyTo(bestTour, 0); // overwrite new tour
                    }
                }
                else
                {
                    foreach (var edge in cur.Kanten)
                    {
                        var other = edge.Other(cur);
                        if (marked[other.ID]) // ignore that then
                            continue;

                        var newCost = tourCost + edge.Weight!.Value;

                        tour[lvl] = edge;
                        permute(lvl + 1, start, other, newCost);
                    }
                }
                // unmark the current node 
                marked[cur.ID] = false;
            }


            var start = graph.Knoten[0];
            var cur = start;

            permute(0, start, start, 0);
            return bestTour.ToList();
        }
    }
}
