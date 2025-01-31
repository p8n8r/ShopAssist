using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShopAssist.ViewModels
{
    internal class DirectionsPageViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;
        private DirectionsPage directionsPage;
        //private Graph directionsGraph; 
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

        public RelayCommand reloadCmd => new RelayCommand(execute => RestartDirections());

        public DirectionsPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        private void RestartDirections()
        {
            Graph directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;
            this.Nodes = new ObservableCollection<ObservableGraphNode>(
                directionsGraph.Nodes.ConvertAll(ObservableGraphNode.NodeToObservableNode));
            this.Edges = new ObservableCollection<ObservableGraphEdge>(
                directionsGraph.Edges.ConvertAll(ObservableGraphEdge.EdgeToObservableEdge));

            this.directionsPage = this.mainWindowViewModel.GetCurrentPage() as DirectionsPage;
            ShowShortestPath();
        }

        private void ShowShortestPath()
        {
            var entranceNode = this.Nodes.FirstOrDefault(n => n.Name == "Entrance");
            var exitNode = this.Nodes.FirstOrDefault(n => n.Name == "Exit");

            if (entranceNode != null && exitNode != null)
            {
                var path = FindShortestPath(entranceNode, exitNode);
                HighlightPath(path);
            }
        }

        public List<GraphNode> FindShortestPath(GraphNode startNode, GraphNode endNode)
        {
            SortedSet<GraphNode> priorityQueue = new SortedSet<GraphNode>(
                Comparer<GraphNode>.Create((n1, n2) => n1.Distance.CompareTo(n2.Distance)));

            startNode.Distance = 0;
            priorityQueue.Add(startNode);

            while (priorityQueue.Any())
            {
                GraphNode currentNode = priorityQueue.Min(); //First()? ??? //pts7
                priorityQueue.Remove(currentNode);

                if (currentNode == null)
                    break;

                if (currentNode.Edges != null)
                {
                    foreach (GraphEdge edge in currentNode.Edges)
                    {
                        GraphNode neighborNode = edge.To;
                        int distanceNew = currentNode.Distance = edge.Weight;

                        if (distanceNew < neighborNode.Distance)
                        {
                            //pts7 ???
                            priorityQueue.Remove(neighborNode);
                            neighborNode.Distance = distanceNew;
                            priorityQueue.Add(neighborNode);
                        }
                    }
                }
            }

            List<GraphNode> path = new List<GraphNode>();
            GraphNode addNode = endNode;

            while (addNode != null)
            {
                path.Add(addNode);
                addNode = addNode.Edges?.OrderBy(e => e.To.Distance).FirstOrDefault()?.To;
            }

            path.Reverse();
            return path;
        }

        private void HighlightPath(IEnumerable<GraphNode> path)
        {
            //this.dataGrid = (this.mainWindowViewModel.GetCurrentPage() as InventoryPage).FindName("inventoryDataGrid") as DataGrid; //pts7 remove

            foreach (GraphNode node in path)
            {
                //var ellipse = directionsPage.MainCanvas.Children.OfType<Ellipse>().FirstOrDefault(
                //    e => ((TextBlock)VisualTreeHelper.GetChild(e, 0)).Text == node.Name);

                //if (ellipse != null)
                //    ellipse.Fill = Brushes.Green;

                //var line = directionsPage.MainCanvas.Children?.OfType<Line>().FirstOrDefault(
                //    l => ((Line)VisualTreeHelper.GetChild(l, 0)).Text == node.Name);

                var lines = directionsPage.MainCanvas.Children?.OfType<Line>();
                //var lines = directionsPage.EdgesCtrl.Children?.OfType<Line>();
                foreach (var line in lines)
                {
                    line.StrokeThickness = 3;
                }
            }
        }
    }
}
