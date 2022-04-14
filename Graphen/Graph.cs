﻿using System.Diagnostics;

namespace Graphen
{
    public class Graph
    {
        public List<Knoten> Knoten;
        public List<Kante> Kanten = new();

        public int KnotenAnzahl => Knoten.Count;

        public Graph(int num)
        {
            Knoten = new List<Knoten>();
            for (var i = 0; i < num; i++)
            {
                var knoten = new Knoten(i);
                Knoten.Add(knoten);
            }
        }

        public static Graph FromTextFile(string file)
        {
            try
            {
                var lines = file.Trim().Split("\n");
                // first line is the amount of nodes
                var amount = int.Parse(lines[0]);
                var graph = new Graph(amount);
                // rest of the lines are the edges in the format "fromID    toID"
                for (var i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var vals = line.Split("\t");
                    Debug.Assert(vals.Length == 2); // only from + to
                    var fromID = int.Parse(vals[0].Trim());
                    var toID = int.Parse(vals[1].Trim());
                    graph.AddKante(new Kante(graph.Knoten[fromID], graph.Knoten[toID]));
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
            var ret = $"#Nodes: {KnotenAnzahl}";
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
        public List<Kante> Kanten = new();

        public Knoten(int id)
        {
            ID = id;
        }

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
            var ret = $"ID: {ID}, #Kanten: {Kanten.Count}";
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