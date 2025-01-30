using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShopAssist.Models
{
    public class GraphNode<T>
    {
        public List<GraphEdge<T>> Edges { get; set; }
        public int Distance { get; set; } = int.MaxValue;
    }
}
