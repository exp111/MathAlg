using System;
using System.Collections.Generic;
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
                foreach (var knoten in graph.Knoten)
                {
                    //TODO: calc a better positioning alg
                    var x = (knoten.ID / rows) * (size + offset);
                    var y = (knoten.ID % rows) * (size + offset);
                    ret += $"<node positionX=\"{x}\" positionY=\"{y}\" id=\"{knoten.ID}\" mainText=\"{knoten.ID}\" upText=\"\" size=\"{size}\"></node>";
                }
                // add edges
                var edgeCount = 0;
                foreach (var kante in graph.Kanten)
                {
                    ret += $"<edge source=\"{kante.Start.ID}\" target=\"{kante.Ende.ID}\" isDirect=\"false\" weight=\"1\" useWeight=\"false\" id=\"{10000 + edgeCount}\" text=\"\" upText=\"\" arrayStyleStart=\"\" arrayStyleFinish=\"\" model_width=\"4\" model_type=\"0\" model_curvedValue=\"0.1\"></edge>";
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
            return new string[0];
        }
    }
}
