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
        private ObservableCollection<ObservableGraphNode> nodes;
        private ObservableCollection<ObservableGraphEdge> edges;

        public ObservableCollection<ObservableGraphNode> Nodes
        {
            get { return nodes; }
            set { if (nodes != value) { nodes = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<ObservableGraphEdge> Edges
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
            this.Nodes = new ObservableCollection<ObservableGraphNode>();
            this.Edges = new ObservableCollection<ObservableGraphEdge>();

            //REPLACE WITH XML DATA
            var entrance = new ObservableGraphNode("Entrance", 50, 50);
            var exit = new ObservableGraphNode("Exit", 150, 50);
            var dairy = new ObservableGraphNode("Dairy", 100, 100);
            var bakery = new ObservableGraphNode("Bakery", 200, 200);
            var produce = new ObservableGraphNode("Produce", 300, 100);

            this.Nodes.Add(entrance);
            this.Nodes.Add(exit);
            this.Nodes.Add(dairy);
            this.Nodes.Add(bakery);
            this.Nodes.Add(produce);

            var edgeEntrance = new ObservableGraphEdge(entrance, bakery, 0);
            var edge0 = new ObservableGraphEdge(entrance, exit, 0);
            var edge1 = new ObservableGraphEdge(dairy, bakery, 1);
            var edge2 = new ObservableGraphEdge(bakery, produce, 2);
            var edge3 = new ObservableGraphEdge(produce, dairy, 3);
            var edgeExit = new ObservableGraphEdge(exit, dairy, 3);

            this.Edges.Add(edgeEntrance);
            this.Edges.Add(edge0);
            this.Edges.Add(edge1);
            this.Edges.Add(edge2);
            this.Edges.Add(edge3);
            this.Edges.Add(edgeExit);
        }

        public List<GraphNode> FindShortestPath(IEnumerable<GraphNode> nodes, 
            GraphNode startNode, GraphNode endNode)
        {
            SortedSet<GraphNode> priorityQueue = new SortedSet<GraphNode>(
                Comparer<GraphNode>.Create((n1, n2) => n1.Distance.CompareTo(n2.Distance)));

            startNode.Distance = 0;
            priorityQueue.Add(startNode);

            while (priorityQueue.Any())
            {
                GraphNode currentNode = priorityQueue.Min();
                priorityQueue.Remove(currentNode);

                if (currentNode == null)
                    break;

                foreach (GraphEdge edge in currentNode.Edges)
                {
                    GraphNode neighborNode = edge.To;
                    int distanceNew = currentNode.Distance = edge.Weight;

                    if (distanceNew < neighborNode.Distance)
                    {
                        priorityQueue.Remove(neighborNode);
                        neighborNode.Distance = distanceNew;
                        priorityQueue.Add(neighborNode);
                    }
                }
            }

            List<GraphNode> path = new List<GraphNode>();
            GraphNode addNode = endNode;

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
