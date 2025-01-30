using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class DirectionsPageViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;
        private ObservableCollection<ObservableGraphNode<Item>> nodes;
        private ObservableCollection<ObservableGraphEdge<Item>> edges;

        public ObservableCollection<ObservableGraphNode<Item>> Nodes
        {
            get { return nodes; }
            set { if (nodes != value) { nodes = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<ObservableGraphEdge<Item>> Edges
        {
            get { return edges; }
            set { if (edges != value) { edges = value; OnPropertyChanged(); } }
        }

        public DirectionsPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            InitializeGraph();
        }

        public void InitializeGraph()
        {
            this.Nodes = new ObservableCollection<ObservableGraphNode<Item>>();
            this.Edges = new ObservableCollection<ObservableGraphEdge<Item>>();

            //REPLACE WITH XML DATA
            var entrance = new ObservableGraphNode<Item>(50, 50);
            var exit = new ObservableGraphNode<Item>(150, 50);
            var dairy = new ObservableGraphNode<Item>(100, 100);
            var bakery = new ObservableGraphNode<Item>(200, 200);
            var produce = new ObservableGraphNode<Item>(300, 100);

            this.Nodes.Add(entrance);
            this.Nodes.Add(exit);
            this.Nodes.Add(dairy);
            this.Nodes.Add(bakery);
            this.Nodes.Add(produce);

            var edgeEntrance = new ObservableGraphEdge<Item>(entrance, bakery, 0);
            var edge0 = new ObservableGraphEdge<Item>(entrance, exit, 0);
            var edge1 = new ObservableGraphEdge<Item>(dairy, bakery, 1);
            var edge2 = new ObservableGraphEdge<Item>(bakery, produce, 2);
            var edge3 = new ObservableGraphEdge<Item>(produce, dairy, 3);
            var edgeExit = new ObservableGraphEdge<Item>(exit, dairy, 3);

            this.Edges.Add(edgeEntrance);
            this.Edges.Add(edge0);
            this.Edges.Add(edge1);
            this.Edges.Add(edge2);
            this.Edges.Add(edge3);
            this.Edges.Add(edgeExit);
        }

        public List<GraphNode<Item>> FindShortestPath(IEnumerable<GraphNode<Item>> nodes, 
            GraphNode<Item> startNode, GraphNode<Item> endNode)
        {
            SortedSet<GraphNode<Item>> priorityQueue = new SortedSet<GraphNode<Item>>(
                Comparer<GraphNode<Item>>.Create((n1, n2) => n1.Distance.CompareTo(n2.Distance)));

            startNode.Distance = 0;
            priorityQueue.Add(startNode);

            while (priorityQueue.Any())
            {
                GraphNode<Item> currentNode = priorityQueue.Min();
                priorityQueue.Remove(currentNode);

                if (currentNode == null)
                    break;

                foreach (GraphEdge<Item> edge in currentNode.Edges)
                {
                    GraphNode<Item> neighborNode = edge.To;
                    int distanceNew = currentNode.Distance = edge.Weight;

                    if (distanceNew < neighborNode.Distance)
                    {
                        priorityQueue.Remove(neighborNode);
                        neighborNode.Distance = distanceNew;
                        priorityQueue.Add(neighborNode);
                    }
                }
            }

            List<GraphNode<Item>> path = new List<GraphNode<Item>>();
            GraphNode<Item> addNode = endNode;

            while (addNode != null)
            {
                path.Add(addNode);
                addNode = addNode.Edges.OrderBy(e => e.To.Distance).FirstOrDefault()?.To;
            }

            path.Reverse();
            return path;
        }
    }
}
