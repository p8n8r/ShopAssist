using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist
{
    public static class ListOperations
    {
        public static void QuickSort<T>(List<T> list) where T : IComparable<T>
        {
            Sort(list, 0, list.Count - 1); //Sort indices of whole list
        }

        private static void Sort<T>(List<T> list, int lower, int upper) where T : IComparable<T>
        {
            if (lower < upper)
            {
                int p = Partition(list, lower, upper); //Index of pivot
                Sort(list, lower, p - 1); //Sort left partition
                Sort(list, p + 1, upper); //Sort right partition
            }
        }

        private static int Partition<T>(List<T> list, int lower, int upper) where T : IComparable<T>
        {
            T pivot = list[upper]; //Choose pivot
            int i = lower - 1;

            //Traverse list, moving smaller items to left
            for (int j = lower; j <= upper - 1; j++)
                if (list[j].CompareTo(pivot) < 0)
                    list.Swap(++i, j);

            //Move pivot index after sorted sublist
            list.Swap(i + 1, upper);

            return i + 1;
        }

        private static void Swap<T>(this List<T> list, int idx1, int idx2)
        {
            (list[idx2], list[idx1]) = (list[idx1], list[idx2]); //Use tuple to swap values
        }
    }
}
