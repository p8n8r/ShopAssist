using ShopAssist.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    public class ObservableGraph : Graph, INotifyPropertyChanged
    {
        public new List<GraphNode> Nodes
        {
            get { return base.Nodes; }
            set { if (base.Nodes != value) { base.Nodes = value; OnPropertyChanged(); } }
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
    }
}
