using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FasterSort
{
    internal class FasterSort
    {
        static readonly Random random = new Random();

        public static T[] Sort<T>(T[] arr, IComparer<T> comparer = null)
        {
            QuickSort(arr, 0, arr.Length - 1, 32, comparer);
            return arr;
        }

        public static List<T> Sort<T>(List<T> list, IComparer<T> comparer = null)
        {
            QuickSort(list, 0, list.Count - 1, 32, comparer);
            return list;
        }

        private static void QuickSort<T>(IList<T> arr, int left, int right, int depth, IComparer<T> comparer)
        {
            if (right - left <= 0) return;

            if (right - left <= 100)
            {
                InsertionSort(arr, left, right, comparer);
            }
            else if (depth == 0 || right - left <= 8192)
            {
                List<T> temp = new List<T>();
                for (int i = left; i <= right; i++) temp.Add(arr[i]);
                temp.Sort(comparer);
                for (int i = 0; i < temp.Count; i++) arr[left + i] = temp[i];
            }
            else
            {
                int pivot = Partition(arr, left, right, comparer);
                Parallel.Invoke(
                    () => QuickSort(arr, left, pivot - 1, depth - 1, comparer),
                    () => QuickSort(arr, pivot + 1, right, depth - 1, comparer)
                );
            }
        }

        private static int Partition<T>(IList<T> arr, int first, int last, IComparer<T> comparer)
        {
            int m = (last - first >> 1) + first;
            int f = (m - first >> 1) + first;
            int l = (last - m >> 1) + m;
            int idx = random.Next(f, l);
            T pivot = arr[idx];
            Swap(arr, idx, first);

            int left = first + 1;
            int right = last;

            while (true)
            {
                while (left <= right && comparer.Compare(arr[left], pivot) < 0) left++;
                while (right >= left && comparer.Compare(arr[right], pivot) > 0) right--;

                if (right < left) break;

                Swap(arr, left, right);
                left++;
                right--;
            }

            Swap(arr, first, right);
            return right;
        }

        private static void InsertionSort<T>(IList<T> arr, int first, int last, IComparer<T> comparer)
        {
            for (int j = first + 1; j <= last; j++)
            {
                T key = arr[j];
                int i = j - 1;
                while (i >= first && comparer.Compare(arr[i], key) > 0)
                {
                    arr[i + 1] = arr[i];
                    i--;
                }
                arr[i + 1] = key;
            }
        }

        private static void Swap<T>(IList<T> arr, int i, int j)
        {
            T temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}