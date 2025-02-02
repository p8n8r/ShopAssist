using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace ShopAssist.Models
{
    [Serializable]
    public class GraphNode : IComparer<GraphNode>, IComparable<GraphNode>
    {
        public string Name {  get; set; }
        [XmlIgnore]
        public List<GraphEdge> Edges { get; set; }
        [XmlIgnore]
        public int Distance { get; set; } = int.MaxValue;
        public int X { get; set; } //Used for display
        public int Y { get; set; } //Used for display

        public GraphNode() //Only used for XML serialization
        {
            this.Edges = new List<GraphEdge>();
        }

        public GraphNode(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Edges = new List<GraphEdge>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int Compare(GraphNode x, GraphNode y)
        {
            return x.Distance.CompareTo(y.Distance);
        }

        public int CompareTo(GraphNode other)
        {
            return this.Distance.CompareTo(other.Distance);
        }
    }
}
