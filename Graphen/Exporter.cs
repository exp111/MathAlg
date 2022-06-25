using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen
{
    public static class Exporter
    {
        // https://graphonline.ru/en/
        // https://github.com/UnickSoft/graphonline
        public static string[] ExportToGraphML(this Graph graph)
        {
            var rows = (int)Math.Floor(Math.Sqrt(graph.KnotenAnzahl));
            var size = 30.0;
            var offset = size;
            try
            {
                var ret = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><graphml>";
                ret += $"<graph id=\"Graph\" uidGraph=\"{graph.KnotenAnzahl}\" uidEdge=\"{10000 + graph.Kanten.Count}\">";
                // add nodes
                foreach (var node in graph.Knoten)
                {
                    //TODO: calc a better positioning alg
                    var x = (node.ID / rows) * (size + offset);
                    var y = (node.ID % rows) * (size + offset);
                    ret += $"<node positionX=\"{x}\" positionY=\"{y}\" id=\"{node.ID}\" mainText=\"{node.ID}\" upText=\"\" size=\"{size}\"></node>";
                }
                // add edges
                var edgeCount = 0;
                foreach (var edge in graph.Kanten)
                {
                    var weight = edge.Weight.HasValue ? edge.Weight.Value.ToString(CultureInfo.InvariantCulture) : "1";
                    ret += $"<edge source=\"{edge.Start.ID}\" target=\"{edge.Ende.ID}\" isDirect=\"false\" weight=\"{weight}\" useWeight=\"false\" id=\"{10000 + edgeCount}\" text=\"\" upText=\"\" arrayStyleStart=\"\" arrayStyleFinish=\"\" model_width=\"4\" model_type=\"0\" model_curvedValue=\"0.1\"></edge>";
                    edgeCount++;
                }
                ret += "</graph>";
                ret += "</graphml>";
                return new string[1] { ret };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during ExportToGraphML: {ex}");
            }
            return Array.Empty<string>();
        }

        // writes the graphml file directly into filename (relative path)
        public static void WriteGraphML(this Graph graph, string filename)
        {
            File.WriteAllLines(filename, graph.ExportToGraphML());
        }
    }
}
