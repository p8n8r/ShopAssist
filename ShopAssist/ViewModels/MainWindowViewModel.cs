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
            //AddJunkData(); // <---- REMOVE THIS 
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

            //REPLACE WITH XML DATA
            var entrance = new GraphNode("Entrance", 50, 50);
            var exit = new GraphNode("Exit", 150, 50);
            var dairy = new GraphNode("Dairy", 100, 100);
            var bakery = new GraphNode("Bakery", 200, 200);
            var produce = new GraphNode("Produce", 300, 100);

            this.Store.DirectionsGraph.Nodes.Add(entrance);
            this.Store.DirectionsGraph.Nodes.Add(exit);
            this.Store.DirectionsGraph.Nodes.Add(dairy);
            this.Store.DirectionsGraph.Nodes.Add(bakery);
            this.Store.DirectionsGraph.Nodes.Add(produce);

            var edgeEntrance = new GraphEdge(entrance, bakery, 0);
            var edge0 = new GraphEdge(entrance, exit, 0);
            var edge1 = new GraphEdge(dairy, bakery, 1);
            var edge2 = new GraphEdge(bakery, produce, 2);
            var edge3 = new GraphEdge(produce, dairy, 3);
            var edgeExit = new GraphEdge(exit, dairy, 3);

            this.Store.DirectionsGraph.Edges.Add(edgeEntrance);
            this.Store.DirectionsGraph.Edges.Add(edge0);
            this.Store.DirectionsGraph.Edges.Add(edge1);
            this.Store.DirectionsGraph.Edges.Add(edge2);
            this.Store.DirectionsGraph.Edges.Add(edge3);
            this.Store.DirectionsGraph.Edges.Add(edgeExit);

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
