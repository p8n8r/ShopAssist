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
    public class ObservableGraphNode<T> : GraphNode<T>, INotifyPropertyChanged
    {
        private int x, y;

        public ObservableGraphNode(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X
        {
            get { return x; }
            set { if (x != value) { x = value; OnPropertyChanged(); } }
        }

        public int Y
        {
            get { return y; }
            set { if (y != value) { y = value; OnPropertyChanged(); } }
        }

        public new int Distance
        {
            get { return base.Distance; }
            set { if (base.Distance != value) { base.Distance = value; OnPropertyChanged(); } }
        }

        public new List<GraphEdge<T>> Edges
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
