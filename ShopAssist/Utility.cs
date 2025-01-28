using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
