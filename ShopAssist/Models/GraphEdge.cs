using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class GraphEdge
    {
        public GraphNode From { get; set; }
        public GraphNode To { get; set; }
        public int Weight { get; set; }

        public GraphEdge() { } //Only used for XML serialization

        public GraphEdge(GraphNode from, GraphNode to, int weight)
        {
            this.From = from;
            this.To = to;
            this.Weight = weight;
        }
    }
}
