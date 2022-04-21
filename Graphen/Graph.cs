using System.Diagnostics;

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

        public static Graph FromTextFile(FileStream file)
        {
            try
            {
                Graph graph;
                int[] edgeCount;
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
                    kante.Start.AddKante(kante);
                    kante.Ende.AddKante(kante);
                }

                return graph;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Graph.FromTextFile: {ex}");
            }

            return new Graph(0);
        }

        public void AddKante(Kante kante)
        {
            Kanten.Add(kante);
            // add references to this edge
            kante.Start.AddKante(kante);
            kante.Ende.AddKante(kante);
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

        public Kante(Knoten start, Knoten ende)
        {
            Start = start;
            Ende = ende;
        }

        public override string ToString()
        {
            return $"From: {Start.ID}, To: {Ende.ID}";
        }
    }
}
