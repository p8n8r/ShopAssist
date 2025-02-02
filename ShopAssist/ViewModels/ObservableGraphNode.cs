using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    public class ObservableGraphNode : GraphNode, INotifyPropertyChanged
    {
        public ObservableGraphNode(string name, int x, int y, List<GraphEdge> edges) : base(name, x, y)
        { 
            this.Edges = edges;
        }

        public new int X
        {
            get { return base.X; }
            set { if (base.X != value) { base.X = value; OnPropertyChanged(); } }
        }

        public new int Y
        {
            get { return base.Y; }
            set { if (base.Y != value) { base.Y = value; OnPropertyChanged(); } }
        }

        public new int Distance
        {
            get { return base.Distance; }
            set { if (base.Distance != value) { base.Distance = value; OnPropertyChanged(); } }
        }

        public new List<GraphEdge> Edges
        {
            get { return base.Edges; }
            set { if (base.Edges != value) { base.Edges = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableGraphNode NodeToObservableNode(GraphNode node)
        {
            return new ObservableGraphNode(node.Name, node.X, node.Y, node.Edges);
        }
    }
}
