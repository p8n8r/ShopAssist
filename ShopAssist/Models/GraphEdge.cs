using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    public class GraphEdge<T>
    {
        public GraphNode<T> From { get; set; }
        public GraphNode<T> To { get; set; }
        public int Weight { get; set; }

        public GraphEdge(GraphNode<T> from, GraphNode<T> to, int weight)
        {
            this.From = from;
            this.To = to;
            this.Weight = weight;
        }
    }
}
