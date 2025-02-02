using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ShopAssist.ViewModels
{
    internal class DirectionsPageViewModel : ViewModelBase
    {
        private const string ENTRANCE = "Entrance", EXIT = "Exit";

        private Random random;
        private MainWindowViewModel mainWindowViewModel;
        private DirectionsPage directionsPage;
        //private Graph directionsGraph; 
        private ObservableCollection<GraphNode> nodes;
        private ObservableCollection<GraphEdge> edges;

        public ObservableCollection<GraphNode> Nodes
        {
            get { return nodes; }
            set { if (nodes != value) { nodes = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<GraphEdge> Edges
        {
            get { return edges; }
            set { if (edges != value) { edges = value; OnPropertyChanged(); } }
        }

        public RelayCommand reloadCmd => new RelayCommand(execute => RestartDirections());
        public RelayCommand nodeClickCmd => new RelayCommand(node => ShowShortestPath(node));

        public DirectionsPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.random = new Random();
            RandomizeGraphWeights();
        }

        private void RestartDirections()
        {
            Graph directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;

            this.Nodes = new ObservableCollection<GraphNode>(directionsGraph.Nodes);
            this.Edges = new ObservableCollection<GraphEdge>(directionsGraph.Edges);

            this.directionsPage = this.mainWindowViewModel.GetCurrentPage() as DirectionsPage;

            RandomizeGraphWeights();
        }

        private void RandomizeGraphWeights()
        {
            Graph directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;
            List<int> idxEdgesChecked = new List<int>();

            foreach (GraphEdge edge in directionsGraph.Edges)
            {
                int idxEdge = directionsGraph.Edges.IndexOf(edge);

                if (!idxEdgesChecked.Contains(idxEdge))
                {
                    edge.Weight = this.random.Next(1, 10);
                    edge.CenterX = (edge.From.X + edge.To.X) / 2;
                    edge.CenterY = (edge.From.Y + edge.To.Y) / 2;

                    idxEdgesChecked.Add(idxEdge);

                    GraphEdge edgeOther = directionsGraph.Edges.Where(
                        e => e.From == edge.To && e.To == edge.From).FirstOrDefault();

                    if (edgeOther != null)
                    {
                        edgeOther.Weight = edge.Weight;
                        edgeOther.CenterX = edge.CenterX;
                        edgeOther.CenterY = edge.CenterY;

                        int idxEdgeOther = directionsGraph.Edges.IndexOf(edge);
                        idxEdgesChecked.Add(idxEdgeOther);
                    }
                }
            }
        }

        private void ShowShortestPath(object node)
        {
            GraphNode selectedNode = node as GraphNode;
            var entranceNode = this.Nodes.FirstOrDefault(n => n.Name == ENTRANCE);
            var exitNode = this.Nodes.FirstOrDefault(n => n.Name == EXIT);

            if (selectedNode != null && entranceNode != null && exitNode != null)
            {
                List<GraphEdge> pathEntranceToNode = FindShortestPath(entranceNode, selectedNode);
                List<GraphEdge> pathNodeToExit = FindShortestPath(selectedNode, exitNode);
                
                ResetHighlightPath();
                HighlightPathToTarget(pathEntranceToNode);
                HighlightPathFromTarget(pathNodeToExit);
            }
        }

        public List<GraphEdge> FindShortestPath(GraphNode startNode, GraphNode endNode)
        {
            Graph directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;
            foreach (GraphNode node in directionsGraph.Nodes)
                node.Edges.First().From.Distance = int.MaxValue;

            SortedSet<GraphNode> priorityQueue = new SortedSet<GraphNode>();
            Dictionary<GraphNode, GraphNode> previousNodes = new Dictionary<GraphNode, GraphNode>();

            startNode.Distance = 0;
            priorityQueue.Add(startNode);

            while (priorityQueue.Any())
            {
                GraphNode currentNode = priorityQueue.First();
                priorityQueue.Remove(currentNode);

                if (currentNode == null)
                    break;

                if (currentNode.Edges != null)
                {
                    foreach (GraphEdge edge in currentNode.Edges)
                    {
                        int distanceNew = currentNode.Distance + edge.Weight;
                        GraphNode neighborNode = edge.To;

                        if (distanceNew < neighborNode.Distance)
                        {
                            priorityQueue.Remove(neighborNode);
                            neighborNode.Distance = distanceNew;
                            previousNodes[neighborNode] = currentNode;
                            priorityQueue.Add(neighborNode);
                        }
                    }
                }
            }

            List<GraphEdge> path = new List<GraphEdge>();
            GraphNode addNode = endNode;

            while (addNode != null && addNode.Name != startNode.Name) //pts7 remove .Name
            {
                var edge = addNode.Edges?.OrderBy(e => e.To.Distance).FirstOrDefault();
                path.Add(edge);
                addNode = edge?.To;
            }
            path.Reverse();

            //List<GraphEdge> path = new List<GraphEdge>();
            //GraphNode addNode = endNode;

            //while (addNode != null && addNode != startNode)
            //{
            //    var previousNode = previousNodes[addNode];
            //    var edge = previousNode?.Edges.FirstOrDefault(e => e.To == addNode);

            //    if (edge == null)
            //        return new List<GraphEdge>(); // No path found

            //    path.Insert(0, edge); // Add the edge to the path
            //    addNode = previousNode;
            //}

            return path;
        }

        private void ResetHighlightPath()
        {
            IEnumerable<Line> lines = Utility.FindVisualChildren<Line>(directionsPage.MainCanvas);

            foreach (var line in lines)
            {
                line.StrokeThickness = 1;
                line.Stroke = Brushes.Black;
            }
        }

        private void HighlightPathToTarget(IEnumerable<GraphEdge> path)
        {
            IEnumerable<Line> lines = Utility.FindVisualChildren<Line>(directionsPage.MainCanvas);

            foreach (GraphEdge edge in path)
            {
                Line lineMatched = lines.Where(l => l.Tag == edge).FirstOrDefault();

                if (lineMatched != null)
                {
                    lineMatched.StrokeThickness = 4;
                    lineMatched.Stroke = Brushes.DarkBlue;
                }
            }
        }

        private void HighlightPathFromTarget(IEnumerable<GraphEdge> path)
        {
            IEnumerable<Line> lines = Utility.FindVisualChildren<Line>(directionsPage.MainCanvas);

            foreach (GraphEdge edge in path)
            {
                Line lineMatched = lines.Where(l => l.Tag == edge).FirstOrDefault();

                if (lineMatched != null)
                {
                    lineMatched.StrokeThickness = 4;
                    lineMatched.Stroke = Brushes.DarkRed;
                }
            }
        }
    }
}
