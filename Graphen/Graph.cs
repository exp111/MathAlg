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
                var knoten = new Knoten(i);
                Knoten.Add(knoten);
            }
        }

        // Makes a new graph from a list of edges. Dies if edges connect to a unknown id
        // Also changes the list you gave into it
        public Graph(int num, List<Kante> edges)
        {
            Kanten = edges;
            Knoten = new(num);
            for (var i = 0; i < num; i++)
            {
                var knoten = new Knoten(i);
                Knoten.Add(knoten);
            }
            // change the edge references to touch this one
            for (var i = 0; i < edges.Count; i++)
            {
                var edge = edges[i];
                edge.Start = Knoten[edge.Start.ID];
                edge.Ende = Knoten[edge.Ende.ID];
            }
        }

        public static Graph FromTextFile(string fileName)
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
                    // rest of the lines are the edges in the format "fromID    toID"
                    while ((line = reader.ReadLine()) != null)
                    {
                        // split by \t, essentially equal to line.Split("\t");
                        var span = line.AsSpan();
                        var index = span.IndexOf("\t");
                        var first = span[..index];
                        var second = span[(index + 1)..];
                        // parse them into ints
                        var fromID = int.Parse(first);
                        var toID = int.Parse(second.TrimEnd()); // trim \r\n from the right side
                        // put the edge into the graph
                        var kante = new Kante(graph.Knoten[fromID], graph.Knoten[toID]);
                        // increase the allocation count
                        edgeCount[fromID]++;
                        edgeCount[toID]++;
                        // only add to the main list rn
                        graph.Kanten.Add(kante);
                    }
                }

                // Now allocate the lists
                foreach (var knoten in graph.Knoten)
                {
                    knoten.Kanten = new(edgeCount[knoten.ID]);
                }

                // then add the edge reference to the nodes
                foreach (var kante in graph.Kanten)
                {
                    kante.AddReference();
                }

                return graph;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Graph.FromTextFile: {ex}");
            }

            return new Graph(0);
        }

        public static Graph FromTextFileWeighted(string fileName)
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
                    // rest of the lines are the edges in the format "fromID    toID    weight"
                    while ((line = reader.ReadLine()) != null)
                    {
                        // split by \t, essentially equal to line.Split("\t");
                        var span = line.AsSpan();
                        var index = span.IndexOf("\t");
                        var first = span[..index];
                        span = span[(index + 1)..]; // move string forward to ignore first \t
                        var secondIndex = span.IndexOf("\t");
                        var second = span[..secondIndex];
                        var third = span[(secondIndex + 1)..];
                        // parse them into ints
                        var fromID = int.Parse(first);
                        var toID = int.Parse(second); // trim \r\n from the right side
                        var weight = double.Parse(third.TrimEnd(), NumberStyles.Float, CultureInfo.InvariantCulture);
                        // put the edge into the graph
                        var kante = new Kante(graph.Knoten[fromID], graph.Knoten[toID], weight);
                        // increase the allocation count
                        edgeCount[fromID]++;
                        edgeCount[toID]++;
                        // only add to the main list rn
                        graph.Kanten.Add(kante);
                    }
                }

                // Now allocate the lists
                foreach (var knoten in graph.Knoten)
                {
                    knoten.Kanten = new(edgeCount[knoten.ID]);
                }

                // then add the edge reference to the nodes
                foreach (var kante in graph.Kanten)
                {
                    kante.AddReference();
                }

                return graph;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Graph.FromTextFileWeighted: {ex}");
            }

            return new Graph(0);
        }

        public void AddKante(Kante kante)
        {
            Kanten.Add(kante);
            // add references to this edge
            kante.AddReference();
        }

        public override string ToString()
        {
            var ret = $"#Knoten: {KnotenAnzahl.ToString("#,##0")}, #Kanten {KantenAnzahl.ToString("#,##0")}";
            ret += "\nKnoten:";
            foreach (var knoten in Knoten)
            {
                ret += $"\n- {knoten}";
            }
            ret += "\nKanten:";
            foreach (var kante in Kanten)
            {
                ret += $"\n- {kante}";
            }
            return ret;
        }
    }

    public class Knoten
    {
        public int ID;
        public List<Kante> Kanten;

        public Knoten(int id)
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

        public void AddKante(Kante kante)
        {
            Kanten.Add(kante);
        }

        public override string ToString()
        {
            var ret = $"ID: {ID}, #Kanten: {KantenAnzahl}";
            foreach (var kante in Kanten)
            {
                ret += $" ({ID}->{(kante.Start.ID == ID ? kante.Ende.ID : kante.Start.ID)})";
            }
            return ret;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Knoten knoten)
                return false;

            return ID == knoten.ID;
        }
    }

    public class Kante
    {
        public Knoten Start;
        public Knoten Ende;
        public double? Weight;
        //TODO: add direction if we need it

        public Kante(Knoten start, Knoten ende, double? weight = null)
        {
            Start = start;
            Ende = ende;
            Weight = weight;
        }

        // Adds itself to the nodes
        public void AddReference()
        {
            Start.Kanten.Add(this);
            Ende.Kanten.Add(this); //TODO: dont add if directed
        }

        // Given a node that touches the edge, returns the node on the other side. Not defined if the node doesnt touch this edge
        public Knoten Other(Knoten k)
        {
            return Start.ID == k.ID ? Ende : Start;
        }

        public override string ToString()
        {
            return $"From: {Start.ID}, To: {Ende.ID}";
        }
    }
}
