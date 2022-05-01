using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static partial class Algorithms
    {
        public static List<Kante> BranchBoundTSPB(this Graph graph)
        {
            var marked = new bool[graph.KnotenAnzahl];
            var bestTourCost = double.MaxValue;
            var bestTour = new Kante[graph.KnotenAnzahl];

            // we can start the tour anywhere because the resulting circle will be that same (as it's a circle)
            var start = graph.Knoten[0];
            var tour = new Kante[graph.KnotenAnzahl];


            // generates all circle tour permutations (in a complete graph) and saves the best one in bestTour
            void permute(int lvl, Knoten cur, double tourCost)
            {
                // mark the current node
                marked[cur.ID] = true;
                // finished this tour? (tour has N edges, last one being back home)
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
                    // run over each edge we havent visited yet
                    foreach (var edge in cur.Kanten)
                    {
                        var other = edge.Other(cur);
                        if (marked[other.ID]) // ignore that then
                            continue;

                        var newCost = tourCost + edge.Weight!.Value;
                        // cancel prematurely if this route is already worse
                        if (bestTourCost < newCost)
                            continue;

                        // add this edge to the tour
                        tour[lvl] = edge;
                        // then run it from the next node at a deeper level
                        permute(lvl + 1, other, newCost);
                    }
                }
                // unmark the current node (as we're going back now)
                marked[cur.ID] = false;
            }

            permute(0, start, 0);
            return bestTour.ToList();
        }
    }
}
