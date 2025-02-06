using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private Graph directionsGraph; 
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
            this.directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;

            RandomizeGraphWeights();
        }

        private void RestartDirections()
        {
            this.directionsGraph = this.mainWindowViewModel.Store.DirectionsGraph;

            this.Nodes = new ObservableCollection<GraphNode>(this.directionsGraph.Nodes);
            this.Edges = new ObservableCollection<GraphEdge>(this.directionsGraph.Edges);

            this.directionsPage = this.mainWindowViewModel.GetCurrentPage() as DirectionsPage;

            RandomizeGraphWeights();
        }

        private void RandomizeGraphWeights()
        {
            List<int> idxEdgesChecked = new List<int>();

            foreach (GraphEdge edge in this.directionsGraph.Edges)
            {
                int idxEdge = this.directionsGraph.Edges.IndexOf(edge);

                if (!idxEdgesChecked.Contains(idxEdge))
                {
                    edge.Weight = this.random.Next(1, 10);
                    edge.CenterX = (edge.From.X + edge.To.X) / 2;
                    edge.CenterY = (edge.From.Y + edge.To.Y) / 2;

                    idxEdgesChecked.Add(idxEdge);

                    GraphEdge edgeOther = this.directionsGraph.Edges.Where(
                        e => e.From == edge.To && e.To == edge.From).FirstOrDefault();

                    if (edgeOther != null)
                    {
                        edgeOther.Weight = edge.Weight;
                        edgeOther.CenterX = edge.CenterX;
                        edgeOther.CenterY = edge.CenterY;

                        int idxEdgeOther = this.directionsGraph.Edges.IndexOf(edge);
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
                HighlightPathFromTarget(pathNodeToExit);
                HighlightPathToTarget(pathEntranceToNode);
            }
        }

        private void ResetEdgeDistances()
        {
            foreach (var node in this.directionsGraph.Nodes)
                node.Distance = int.MaxValue;
        }

        public List<GraphEdge> FindShortestPath(GraphNode startNode, GraphNode endNode)
        {
            ResetEdgeDistances();

            SortedSet<GraphNode> priorityQueue = new SortedSet<GraphNode>();
            Dictionary<string, GraphNode> previousNodes = new Dictionary<string, GraphNode>();
            HashSet<GraphNode> visitedNodes = new HashSet<GraphNode>();

            startNode.Distance = 0;
            priorityQueue.Add(startNode);

            while (priorityQueue.Any())
            {
                GraphNode currentNode = priorityQueue.First();
                priorityQueue.Remove(currentNode);
                visitedNodes.Add(currentNode);

                if (currentNode.Edges != null)
                {
                    foreach (GraphEdge edge in currentNode.Edges)
                    {
                        int distanceNew = currentNode.Distance + edge.Weight;
                        GraphNode neighborNode = edge.To;

                        if (visitedNodes.Contains(neighborNode)) { continue; }

                        if (distanceNew < neighborNode.Distance)
                        {
                            priorityQueue.Remove(neighborNode);
                            neighborNode.Distance = distanceNew;
                            previousNodes[neighborNode.Name] = currentNode;
                            priorityQueue.Add(neighborNode);
                        }
                    }
                }
            }

            List<GraphEdge> path = new List<GraphEdge>();
            GraphNode addNode = endNode;

            while (addNode != null && addNode != startNode)
            {
                if (!previousNodes.TryGetValue(addNode.Name, out var previousNode))
                    return new List<GraphEdge>(); // No path found

                var edge = previousNode?.Edges.FirstOrDefault(e => e.To == addNode);

                if (edge == null)
                    return new List<GraphEdge>(); // No path found

                path.Insert(0, edge); // Add the edge to the path
                addNode = previousNode;
            }

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
