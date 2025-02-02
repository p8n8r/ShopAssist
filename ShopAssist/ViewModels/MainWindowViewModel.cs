using ShopAssist.DisplayDialogs;
using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;
using System.Xml.Serialization;

namespace ShopAssist.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private const string STORE_FILE = @".\Datasets\Store.xml";
        public readonly IDisplayDialog displayDialog;

        public Store Store { get; set; }

        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } }
        public RelayCommand customersCmd => new RelayCommand(execute => OpenCustomerPage());
        public RelayCommand inventoryCmd => new RelayCommand(execute => OpenInventoryPage());
        public RelayCommand categoriesCmd => new RelayCommand(execute => OpenCategoryPage());
        public RelayCommand checkoutCmd => new RelayCommand(execute => OpenCheckoutPage());
        public RelayCommand directionsCmd => new RelayCommand(execute => OpenDirectionsPage());
        public RelayCommand onCloseCmd => new RelayCommand(execute => OnClose());

        public MainWindowViewModel(MainWindow mainWindow, IDisplayDialog displayDialog)
        {
            this.mainWindow = mainWindow;
            this.displayDialog = displayDialog;

            ImportStore(STORE_FILE);
            AddJunkData(); // <---- REMOVE THIS 
        }

        private void AddJunkData() // <---- REMOVE THIS 
        {
            //Add junk data
            this.Store.Customers = new List<Customer>()
            {
                new Customer() { Name = "Peyton Stults", Membership = new Membership() { Id = 1, MembershipLevel = MembershipLevel.High} },
                new Customer() { Name = "Addison Stults", Membership = new Membership() { Id = 2, MembershipLevel = MembershipLevel.Medium} },
                new Customer() { Name = "Parker Stults", Membership = new Membership() { Id = 3, MembershipLevel = MembershipLevel.Low} },
                new Customer() { Name = "Will Stults", Membership = new Membership() { Id = 4, MembershipLevel = MembershipLevel.No} }
            };

            this.Store.Inventory = new Dictionary<int, Item>()
            {
                { 0, new Item() { Name = "Apple", Category = "Fruit", Stock = 10, Price = 0.89M, Code = 0 } },
                { 1, new Item() { Name = "Steak", Category = "Meat", Stock = 3, Price = 8.99M, Code = 1 } },
                { 2, new Item() { Name = "Butter", Category = "Dairy", Stock = 5, Price = 3.56M, Code = 2 } }
            };

            this.Store.Categories = new Tree<Category>() { Root = new TreeNode<Category>() { Data = new Category() { Name = "All" } } };
            this.Store.Categories.Root.Children = new List<TreeNode<Category>>()
            {
                new TreeNode<Category>()
                {
                    Data = new Category() { Name = "Meats" },
                    Parent = this.Store.Categories.Root,
                    Children = new List<TreeNode<Category>>()
                    {
                        new TreeNode<Category>()
                        {
                            Data = new Category() { Name = "Poultry" },
                            //Parent = this.Store.Categories.Root.Children[0]
                        },
                        new TreeNode<Category>()
                        {
                            Data = new Category() { Name = "Steak" },
                            //Parent = this.Store.Categories.Root.Children[0]
                        },
                    }
                },
                new TreeNode<Category>()
                {
                    Data = new Category() { Name = "Fruits and Vegetables" },
                    //Parent = this.Store.Categories.Root,
                    Children = new List<TreeNode<Category>>()
                    {
                        new TreeNode<Category>()
                        {
                            Data = new Category() { Name = "Fruit" },
                            //Parent = this.Store.Categories.Root.Children[1],
                            Children = new List<TreeNode<Category>>()
                            {
                                new TreeNode<Category>()
                                {
                                    Data = new Category() { Name = "Apple" },
                                    //Parent = this.Store.Categories.Root.Children[1].Children[0]
                                },
                                new TreeNode<Category>()
                                {
                                    Data = new Category() { Name = "Banana" },
                                    //Parent = this.Store.Categories.Root.Children[1].Children[0]
                                }
                            }
                        },
                        new TreeNode<Category>()
                        {
                            Data = new Category() { Name = "Vegetables" },
                            //Parent = this.Store.Categories.Root.Children[1],
                            Children = new List<TreeNode<Category>>()
                            {
                                new TreeNode<Category>()
                                {
                                    Data = new Category() { Name = "Broccoli" },
                                    //Parent = this.Store.Categories.Root.Children[1].Children[1]
                                },
                                new TreeNode<Category>()
                                {
                                    Data = new Category() { Name = "Carrot" },
                                    //Parent = this.Store.Categories.Root.Children[1].Children[1]
                                }
                            }
                        }
                    }
                }
            };

            this.Store.Categories.Root.Children[0].Children[0].Parent = this.Store.Categories.Root.Children[0];
            this.Store.Categories.Root.Children[0].Children[1].Parent = this.Store.Categories.Root.Children[0];

            this.Store.DirectionsGraph = new Graph();

            //REPLACE WITH XML DATA  X: 50 -> 600, Y: 25 -> 300
            var entrance = new GraphNode("Entrance", 25, 100);
            var exit = new GraphNode("Exit", 25, 200);

            var dairy = new GraphNode("Dairy", 125, 75);
            var bakery = new GraphNode("Bakery", 125, 175);
            var fruits = new GraphNode("Fruits", 125, 275);
            var vegs = new GraphNode("Veggies", 225, 50);
            var meats = new GraphNode("Meat", 225, 155);
            var drinks = new GraphNode("Drinks", 225, 255);
            var snacks = new GraphNode("Snacks", 325, 25);
            var pastas = new GraphNode("Pasta", 325, 150);
            var desserts = new GraphNode("Desserts", 325, 275);
            var candy = new GraphNode("Candy", 425, 200);
            var seafood = new GraphNode("Seafood", 425, 300);
            var pizza = new GraphNode("Pizza", 425, 75);
            var nuts = new GraphNode("Nuts", 550, 50);
            var crackers = new GraphNode("Crackers", 550, 150);
            var cereal = new GraphNode("Cereal", 550, 275);

            this.Store.DirectionsGraph.Nodes.Add(entrance);
            this.Store.DirectionsGraph.Nodes.Add(exit);
            this.Store.DirectionsGraph.Nodes.Add(dairy);
            this.Store.DirectionsGraph.Nodes.Add(bakery);
            this.Store.DirectionsGraph.Nodes.Add(fruits);
            this.Store.DirectionsGraph.Nodes.Add(vegs);
            this.Store.DirectionsGraph.Nodes.Add(meats);
            this.Store.DirectionsGraph.Nodes.Add(drinks);
            this.Store.DirectionsGraph.Nodes.Add(snacks);
            this.Store.DirectionsGraph.Nodes.Add(pastas);
            this.Store.DirectionsGraph.Nodes.Add(desserts);
            this.Store.DirectionsGraph.Nodes.Add(candy);
            this.Store.DirectionsGraph.Nodes.Add(seafood);
            this.Store.DirectionsGraph.Nodes.Add(pizza);
            this.Store.DirectionsGraph.Nodes.Add(nuts);
            this.Store.DirectionsGraph.Nodes.Add(crackers);
            this.Store.DirectionsGraph.Nodes.Add(cereal);

            entrance.Edges = new List<GraphEdge>() 
            {
                new GraphEdge(entrance, bakery, 0),
                new GraphEdge(entrance, exit, 0),
                new GraphEdge(entrance, dairy, 0)
            };

            exit.Edges = new List<GraphEdge>() 
            {
                new GraphEdge(exit, entrance, 0),
                new GraphEdge(exit, dairy, 0),
                new GraphEdge(exit, bakery, 0) 
            };

            dairy.Edges = new List<GraphEdge>()
            {
                new GraphEdge(dairy, entrance, 0),
                new GraphEdge(dairy, exit, 0),
                new GraphEdge(dairy, bakery, 0),
                new GraphEdge(dairy, vegs, 0),
                new GraphEdge(dairy, meats, 0)
            };

            bakery.Edges = new List<GraphEdge>()
            {
                new GraphEdge(bakery, entrance, 0),
                new GraphEdge(bakery, exit, 0),
                new GraphEdge(bakery, dairy, 0),
                new GraphEdge(bakery, fruits, 0),
                new GraphEdge(bakery, meats, 0),
                new GraphEdge(bakery, drinks, 0)
            };

            fruits.Edges = new List<GraphEdge>()
            {
                new GraphEdge(fruits, exit, 0),
                new GraphEdge(fruits, bakery, 0),
                new GraphEdge(fruits, meats, 0),
                new GraphEdge(fruits, drinks, 0)
            };

            vegs.Edges = new List<GraphEdge>()
            {
                new GraphEdge(vegs, dairy, 0),
                new GraphEdge(vegs, bakery, 0),
                new GraphEdge(vegs, meats, 0),
                new GraphEdge(vegs, snacks, 0),
                new GraphEdge(vegs, pastas, 0)
            };

            meats.Edges = new List<GraphEdge>()
            {
                new GraphEdge(meats, dairy, 0),
                new GraphEdge(meats, bakery, 0),
                new GraphEdge(meats, fruits, 0),
                new GraphEdge(meats, vegs, 0),
                new GraphEdge(meats, drinks, 0),
                new GraphEdge(meats, snacks, 0),
                new GraphEdge(meats, pastas, 0),
                new GraphEdge(meats, desserts, 0)
            };

            drinks.Edges = new List<GraphEdge>()
            {
                new GraphEdge(drinks, bakery, 0),
                new GraphEdge(drinks, fruits, 0),
                new GraphEdge(drinks, meats, 0),
                new GraphEdge(drinks, drinks, 0),
                new GraphEdge(drinks, pastas, 0),
                new GraphEdge(drinks, desserts, 0)
            };

            snacks.Edges = new List<GraphEdge>()
            {
                new GraphEdge(snacks, vegs, 0),
                new GraphEdge(snacks, meats, 0),
                new GraphEdge(snacks, pastas, 0),
                new GraphEdge(snacks, pizza, 0)
            };

            pastas.Edges = new List<GraphEdge>()
            {
                new GraphEdge(pastas, vegs, 0),
                new GraphEdge(pastas, meats, 0),
                new GraphEdge(pastas, drinks, 0),
                new GraphEdge(pastas, snacks, 0),
                new GraphEdge(pastas, desserts, 0),
                new GraphEdge(pastas, pizza, 0),
                new GraphEdge(pastas, candy, 0),
                new GraphEdge(pastas, seafood, 0)
            };

            desserts.Edges = new List<GraphEdge>()
            {
                new GraphEdge(desserts, meats, 0),
                new GraphEdge(desserts, drinks, 0),
                new GraphEdge(desserts, snacks, 0),
                new GraphEdge(desserts, pastas, 0),
                new GraphEdge(desserts, candy, 0),
                new GraphEdge(desserts, seafood, 0)
            };

            pizza.Edges = new List<GraphEdge>()
            {
                new GraphEdge(pizza, snacks, 0),
                new GraphEdge(pizza, pastas, 0),
                new GraphEdge(pizza, candy, 0),
                new GraphEdge(pizza, nuts, 0),
                new GraphEdge(pizza, crackers, 0)
            };

            candy.Edges = new List<GraphEdge>()
            {
                new GraphEdge(candy, snacks, 0),
                new GraphEdge(candy, pastas, 0),
                new GraphEdge(candy, desserts, 0),
                new GraphEdge(candy, pizza, 0),
                new GraphEdge(candy, seafood, 0),
                new GraphEdge(candy, nuts, 0),
                new GraphEdge(candy, crackers, 0)
            };

            seafood.Edges = new List<GraphEdge>()
            {
                new GraphEdge(seafood, pastas, 0),
                new GraphEdge(seafood, desserts, 0),
                new GraphEdge(seafood, candy, 0),
                new GraphEdge(seafood, crackers, 0),
                new GraphEdge(seafood, cereal, 0)
            };

            nuts.Edges = new List<GraphEdge>()
            {
                new GraphEdge(nuts, pizza, 0),
                new GraphEdge(nuts, candy, 0),
                new GraphEdge(nuts, crackers, 0)
            };

            crackers.Edges = new List<GraphEdge>()
            {
                new GraphEdge(crackers, pizza, 0),
                new GraphEdge(crackers, candy, 0),
                new GraphEdge(crackers, seafood, 0),
                new GraphEdge(crackers, nuts, 0),
                new GraphEdge(crackers, cereal, 0)
            };

            cereal.Edges = new List<GraphEdge>()
            {
                new GraphEdge(cereal, candy, 0),
                new GraphEdge(cereal, seafood, 0),
                new GraphEdge(cereal, crackers, 0)
            };

            foreach (GraphNode node in this.Store.DirectionsGraph.Nodes)
                this.Store.DirectionsGraph.Edges.AddRange(node.Edges);

            //this.Store.DirectionsGraph.Edges.AddRange(edges);

            //...
        }

        private void ImportStore(string filePath)
        {
            if (File.Exists(filePath))
                this.Store = ImportXmlFile(typeof(Store), filePath) as Store;

            if (this.Store != null) //Data found?
            {
                //Let's restore the inventory dictionay,
                //this time with a new ID number to get
                //around that IDictionary serialization issue.
                for (int i = 0; i < this.Store.Items.Count; i++)
                    this.Store.Inventory[i] = this.Store.Items[i];

                //Let's restore the category tree parents.
                //The parents have to be left out 
                //of XML serialization.
                this.Store.Categories.RestoreParents();

                //Let's retore the node's edges.
                //The edges had to be left out of XML
                //serialization dure to circular references.
                foreach (GraphEdge edge in this.Store.DirectionsGraph.Edges)
                    edge.From.Edges.Add(new GraphEdge(edge.From, edge.To, 0));
            }
            else
            {
                this.Store = new Store(); //Create empty store
            }
        }

        private void ExportStore(string filePath)
        {
            if (this.Store != null) //Data found?
            {
                //Save Dictionary as List to avoid serialization problems 
                //with classes that implement IDictionary. We'll assign
                //new IDs on next startup to get around this issue.
                this.Store.Items = this.Store.Inventory.Select(p => p.Value).ToList();
            }
            else
            {
                this.Store = new Store(); //Create empty store
            }

            ExportAsXmlFile(this.Store, typeof(Store), filePath);
        }

        private object ImportXmlFile(Type dataType, string filePath)
        {
            object data = null;

            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer serializer = new XmlSerializer(dataType);

                using (stream)
                {
                    data = serializer.Deserialize(stream);
                    Convert.ChangeType(data, dataType);
                }
            }
            //catch (Exception e)
            //{
            //    displayDialog.ShowErrorMessageBox(e.Message);
            //    throw; //?
            //}
            catch (ArgumentNullException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentOutOfRangeException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (NotSupportedException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FileNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (UnauthorizedAccessException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (DirectoryNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (PathTooLongException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (IOException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (SecurityException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (InvalidOperationException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (InvalidCastException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FormatException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (OverflowException e) { displayDialog.ShowErrorMessageBox(e.Message); }

            return data;
        }

        private void ExportAsXmlFile(object data, Type dataType, string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            try
            {
                XmlSerializer serializer = new XmlSerializer(dataType);

                using (stream)
                {
                    serializer.Serialize(stream, data);
                }
            }
            //catch (Exception e)
            //{
            //    displayDialog.ShowErrorMessageBox(e.Message);
            //    throw; //?
            //}
            catch (ArgumentNullException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentOutOfRangeException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (NotSupportedException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FileNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (UnauthorizedAccessException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (DirectoryNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (PathTooLongException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (IOException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (SecurityException e) { displayDialog.ShowErrorMessageBox(e.Message); }
        }

        private void OpenCustomerPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.customerPage);
        }

        private void OpenInventoryPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.inventoryPage);
        }
        
        private void OpenCategoryPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.categoryPage);
        }

        private void OpenCheckoutPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.checkoutPage);
        }

        private void OpenDirectionsPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.directionsPage);
        }

        public Page GetCurrentPage()
        {
            if (this.MainWindow != null)
                return (this.MainWindow.FindName("mainFrame") as Frame).Content as Page;

            return null;
        }

        private void OnClose()
        {
            ExportStore(STORE_FILE);
        }
    }
}
