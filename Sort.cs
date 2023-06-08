using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSort
{

    public class Sort<T> // Класс собственных сортировок
    {

        public delegate int CallbackSort(T x, T y, int ix, int iy); // Иногда бывает, что сравнения в сортировках происходят с какими-то заранее выбранными величинами, а не элементами массива, поэтому стандартными компораторами тут не обойтись. Надо делать callback-функции

        public static void BubbleSort(ref T[] arr, CallbackSort callbackCompare) // Сортировка пузырьком
        {
            T temp;
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (callbackCompare(arr[i],arr[j],i,j) > 0)
                    {
                        temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }

        public static void SelectionSort(ref T[] arr, CallbackSort callbackCompare) // Сортировка выбором
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int imax = 0;
                for (int j = 0; j <= arr.Length - i; j++)
                {
                    if (callbackCompare(arr[j], arr[imax], j, imax) > 0)
                    {
                        imax = j;
                    }
                }

                T tmp = arr[imax];
                arr[imax] = arr[arr.Length - i];
                arr[arr.Length - i] = tmp;
            }
        }

        public static void InsectionSort(ref T[] arr, int begin, int end, CallbackSort callbackCompare)
        { // Сортировка вставками
            for (int i = begin + 1; i <= end; i++)
            {
                int j = i;
                T t = arr[i];
                while (j > 0 && callbackCompare(arr[j - 1], t, j-1, i) > 0)
                {
                    arr[j] = arr[j - 1];
                    j = j - 1;
                }
                arr[j] = t;
            }
        }

        public static void QuickSort(ref T[] arr, int begin, int end, CallbackSort callbackCompare) // Быстрая сортировка
        {
            int l = begin, r = end; // Сохраняем края сортируемого массива

            T piv; // Объявляем опорный элемент
            int iPiv;
            Random rand= new Random();
            int iTmp1 = rand.Next(l, r);
            int iTmp2 = rand.Next(l, r);
            int iTmp3 = rand.Next(l, r);
            if (callbackCompare(arr[iTmp1], arr[iTmp2], iTmp1, iTmp2) > 0 && callbackCompare(arr[iTmp3], arr[iTmp2], iTmp3, iTmp2) > 0)
            {
                if (callbackCompare(arr[iTmp3], arr[iTmp1], iTmp3, iTmp1) > 0)
                {
                    piv = arr[iTmp1];
                    iPiv = iTmp1;
                }
                else
                {
                    piv = arr[iTmp3];
                    iPiv = iTmp3;
                }
            }
            else
            {
                if (callbackCompare(arr[iTmp1], arr[iTmp3], iTmp1, iTmp3) > 0)
                {
                    piv = arr[iTmp1];
                    iPiv = iTmp1;
                }
                else
                {
                    piv = arr[iTmp3];
                    iPiv = iTmp3;
                }
            }
            while (l <= r)
            { // Затем идем по массиву до тех пор, пока правый край не встретится с левым
                while (callbackCompare(piv, arr[l], iPiv, l) > 0) // Если piv>arr[l], то все хорошо - сдвигаем левый край на единицу вправо
                    l++;
                while (callbackCompare(arr[r], piv, r, iPiv) > 0) // Если piv<arr[r], то тоже все хорошо
                    r--;
                if (l <= r)
                { // Если правый край еще не встретился с левым, причем arr[r]>=piv, а arr[l]<=piv
                  // т.е. мы нашли элементы, которые стоят не там, где надо (у нас правее опорного все
                  // элементы должны быть больше него, а левее опорного - меньше его)
                  // Меняем местами элементы на краях сортируемого массива (те элементы, что стоят не там, где надо)
                    T tmp = arr[l];
                    arr[l] = arr[r];
                    arr[r] = tmp;
                    // Элементы на краях обработаны
                    l++;
                    r--;
                }
            }
            if (begin < r) // Если это еще возможно - сортируем левую часть массива (которая до опорного)
                QuickSort(ref arr, begin, r, callbackCompare);
            if (end > l) // Если это возможно - сортируем правую часть массива
                QuickSort(ref arr, l, end, callbackCompare);
        }
    }
}
