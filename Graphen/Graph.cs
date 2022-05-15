using System.Diagnostics;
using System.Globalization;

namespace Graphen
{
    public class Graph
    {
        public List<Knoten> Knoten;
        public List<Kante> Kanten;

        public int KnotenAnzahl => Knoten.Count;
        public int KantenAnzahl => Kanten.Count;

        public Graph(int num, int edgeCount = default)
        {
            Kanten = new(edgeCount);
            Knoten = new(num);
            for (var i = 0; i < num; i++)
            {
                var node = new Knoten(i);
                Knoten.Add(node);
            }
        }

        // Makes a new graph from a list of edges. Dies if edges connect to a unknown id
        public Graph(int num, List<Kante> edges)
        {
            Kanten = new(edges.Count);
            Knoten = new(num);
            for (var i = 0; i < num; i++)
            {
                var node = new Knoten(i);
                Knoten.Add(node);
            }
            int[] edgeCount = new int[num];

            // copy the edges over (by creating new nodes)
            foreach (var edge in edges)
            {
                Kanten.Add(new Kante(Knoten[edge.Start.ID], Knoten[edge.Ende.ID], edge.Weight, edge.Directed));
                // increase allocation count
                edgeCount[edge.Start.ID]++;
                if (!edge.Directed)
                    edgeCount[edge.Ende.ID]++;
            }

            // allocate the lists so we dont have to up the capacity
            foreach (var node in Knoten)
            {
                node.Kanten = new(edgeCount[node.ID]);
            }

            // then add the edge reference to the nodes
            foreach (var edge in Kanten)
            {
                edge.AddReference();
            }
        }

        public static Graph FromTextFile(string fileName, bool directed = false)
        {
            try
            {
                Graph graph;
                int[] edgeCount;
                var file = File.OpenRead(fileName);
                using (var reader = new StreamReader(file))
                {
                    // first line is the amount of nodes
                    var line = reader.ReadLine()!.TrimEnd();
                    var amount = int.Parse(line);
                    graph = new Graph(amount);//, lines.Length - 1);
                    // Contains the edgecount for each node, so we can allocate them in one batch instead of needing to resize
                    edgeCount = new int[amount];
                    // rest of the lines are the edges in the format "fromID    toID    weight" (weight being optional)
                    while ((line = reader.ReadLine()) != null)
                    {
                        // split by \t, essentially equal to line.Split("\t");
                        var span = line.AsSpan();
                        var index = span.IndexOf("\t");
                        var first = span[..index];
                        span = span[(index + 1)..]; // move string forward to ignore first \t
                        var secondIndex = span.IndexOf("\t");
                        ReadOnlySpan<char> second = span;
                        double? weight = null;
                        if (secondIndex != -1) // found another \t => line got weight
                        {
                            second = span[..secondIndex];
                            var third = span[(secondIndex + 1)..];
                            weight = double.Parse(third.TrimEnd(), NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                        
                        // parse them into ints
                        var fromID = int.Parse(first);
                        var toID = int.Parse(second); // trim \r\n from the right side
                        // put the edge into the graph
                        var edge = new Kante(graph.Knoten[fromID], graph.Knoten[toID], weight, directed);
                        // increase the allocation count
                        edgeCount[fromID]++;
                        edgeCount[toID]++;
                        // only add to the main list rn
                        graph.Kanten.Add(edge);
                    }
                }

                // Now allocate the lists
                foreach (var node in graph.Knoten)
                {
                    node.Kanten = new(edgeCount[node.ID]);
                }

                // then add the edge reference to the nodes
                foreach (var edge in graph.Kanten)
                {
                    edge.AddReference();
                }

                return graph;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Graph.FromTextFile: {ex}");
            }

            return new Graph(0);
        }

        public void AddKante(Kante edge)
        {
            Kanten.Add(edge);
            // add references to this edge
            edge.AddReference();
        }

        public Kante? GetEdge(int startID, int endID)
        {
            var start = Knoten[startID];
            var end = Knoten[endID];
            return start.GetEdge(end);
        }

        public override string ToString()
        {
            var ret = $"#Knoten: {KnotenAnzahl.ToString("#,##0")}, #Kanten {KantenAnzahl.ToString("#,##0")}";
            ret += "\nKnoten:";
            foreach (var node in Knoten)
            {
                ret += $"\n- {node}";
            }
            ret += "\nKanten:";
            foreach (var edge in Kanten)
            {
                ret += $"\n- {edge}";
            }
            return ret;
        }
    }

    public class Knoten
    {
        public int ID;
        public List<Kante> Kanten;

        internal Knoten(int id)
        {
            ID = id;
        }

        public Knoten(int id, int allocate)
        {
            ID = id;
            Kanten = new(allocate);
        }

        public int KantenAnzahl => Kanten == null ? Kanten!.Count : 0;

        public static bool operator ==(Knoten a, Knoten b)
            => a.ID == b.ID;

        public static bool operator !=(Knoten a, Knoten b)
            => a.ID != b.ID;

        public void AddKante(Kante edge)
        {
            Kanten.Add(edge);
        }

        // Returns the edge between this and the given node or null if it doesn't exist
        public Kante? GetEdge(Knoten node)
        {
            foreach (var edge in Kanten)
            {
                if (edge.Other(this) == node)
                {
                    return edge;
                }
            }

            return null;
        }

        public override string ToString()
        {
            var ret = $"ID: {ID}, #Kanten: {KantenAnzahl}";
            foreach (var node in Kanten)
            {
                ret += $" ({ID}->{(node.Start.ID == ID ? node.Ende.ID : node.Start.ID)})";
            }
            return ret;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Knoten node)
                return false;

            return ID == node.ID;
        }

        public static readonly int InvalidID = -1; //TODO: maybe do null or NaN?
    }

    public class Kante
    {
        public Knoten Start;
        public Knoten Ende;
        public double? Weight;
        public bool Directed;

        public Kante(Knoten start, Knoten end, double? weight = null, bool directed = false)
        {
            Start = start;
            Ende = end;
            Weight = weight;
            Directed = directed;
        }

        // Adds itself to the nodes
        public void AddReference()
        {
            Start.Kanten.Add(this);
            if (!Directed)
                Ende.Kanten.Add(this);
        }

        // Given a node that touches the edge, returns the node on the other side. Not defined if the node doesnt touch this edge
        public Knoten Other(Knoten k)
        {
            return Start.ID == k.ID ? Ende : Start;
        }

        public override string ToString()
        {
            if (Weight.HasValue)
                return $"From: {Start.ID}, To: {Ende.ID}, Weight: {Weight}, Directed: {Directed}";
            else
                return $"From: {Start.ID}, To: {Ende.ID}, Directed: {Directed}";
        }
    }

    public static class KantenListExtension
    {
        public static double GetWeight(this List<Kante> edges)
        {
            var weight = 0d;
            foreach (var edge in edges)
            {
                weight += edge.Weight!.Value;
            }
            return weight;
        }

        public static string GetPath(this List<Kante> edges)
        {
            var cur = edges[0].Start;
            var path = cur.ID.ToString();
            foreach (var edge in edges)
            {
                var other = edge.Other(cur);
                path += $"->{other.ID}";
                cur = other;
            }
            return path;
        }
    }

    public class ShortestPathTree
    {
        public Graph Graph;
        public double[] Dist;
        public int[] Pred;

        public ShortestPathTree(Graph graph, double[] dist, int[] pred)
        {
            Graph = graph;
            Dist = dist;
            Pred = pred;
        }

        public List<Kante>? GetShortestPath(int endID)
        {
            List<Kante> queue = new();
            var cur = endID;
            int next = Pred[cur];
            while (cur != next)
            {
                if (next == Knoten.InvalidID)
                    return null;

                queue.Add(Graph.GetEdge(next, cur)!);
                cur = next;
                next = Pred[cur];
            }
            return queue;
        }

        public double GetShortestPathWeight(int endID)
        {
            return Dist[endID];
        }
    }
}
