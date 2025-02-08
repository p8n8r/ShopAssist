using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class Graph
    {
        public List<GraphNode> Nodes { get; set; }
        public List<GraphEdge> Edges { get; set; }

        public Graph()
        {
            this.Nodes = new List<GraphNode>();
            this.Edges = new List<GraphEdge>();
        }
    }
}
