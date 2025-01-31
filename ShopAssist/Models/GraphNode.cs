using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShopAssist.Models
{
    [Serializable]
    public class GraphNode
    {
        public string Name {  get; set; }
        public List<GraphEdge> Edges { get; set; }
        public int Distance { get; set; } = int.MaxValue;
        public int X { get; set; }
        public int Y { get; set; }

        public GraphNode() { } //Only used for XML serialization

        public GraphNode(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }
    }
}
