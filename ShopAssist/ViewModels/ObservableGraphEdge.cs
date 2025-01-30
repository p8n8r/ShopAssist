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
    public class ObservableGraphEdge<T> : GraphEdge<T>, INotifyPropertyChanged
    {
        public ObservableGraphEdge(GraphNode<T> from, GraphNode<T> to, int weight) : base(from, to, weight) { }

        public new GraphNode<T> From
        {
            get { return base.From; }
            set { if (base.From != value) { base.From = value; OnPropertyChanged(); } }
        }

        public new GraphNode<T> To
        {
            get { return base.To; }
            set { if (base.To != value) { base.To = value; OnPropertyChanged(); } }
        }

        public new int Weight
        {
            get { return base.Weight; }
            set { if (base.Weight != value) { base.Weight = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
