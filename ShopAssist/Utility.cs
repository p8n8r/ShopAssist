using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ShopAssist
{
    public static class Utility
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
        {
            List<T> listSorted = collection.OrderBy(i => i).ToList();

            for (int i = 0; i < listSorted.Count; i++)
                collection.Move(collection.IndexOf(listSorted[i]), i);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T tChild)
                    {
                        yield return tChild;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
