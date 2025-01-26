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
            Sort(list, 0, list.Count - 1);
        }

        private static void Sort<T>(List<T> list, int lower, int upper) where T : IComparable<T>
        {
            if (lower < upper)
            {
                int p = Partition(list, lower, upper); //Index of pivot
                Sort(list, lower, p);
                Sort(list, p + 1, upper);
            }
        }

        private static int Partition<T>(List<T> list, int lower, int upper) where T : IComparable<T>
        {
            int i = lower, j = upper;
            T pivot = list[lower];

            do
            {
                while (list[i].CompareTo(pivot) < 0) { ++i; }
                while (list[j].CompareTo(pivot) > 0) { --j; }
                if (i >= j) break;
                list.Swap(i, j);
            }
            while (i <= j);

            return j;
        }

        private static void Swap<T>(this List<T> list, int idx1, int idx2)
        {
            (list[idx2], list[idx1]) = (list[idx1], list[idx2]); //Use tuple to swap values
        }
    }
}
