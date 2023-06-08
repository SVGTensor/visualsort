using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace VisualSort
{

    public partial class sortControl1 : UserControl
    {
        static Brush brushBack = new SolidBrush(Color.White);
        static Brush brushRectangle = new SolidBrush(Color.Gray);
        static Brush activeRectangle = new SolidBrush(Color.Red);
        static Pen penRectangle = new Pen(Color.Black);

        private static int[] numbers; // Сортируемый массив
        public static int activeIndex1; // Индекс первого активного элемента (участвующего на данном шаге в сравнении), используется компоратором. Если равен -1, то выделенных прямоугольников не будет
        public static int activeIndex2; // Индекс второго активного элемента (участвующего в данном шаге в сравнении), используется компоратором. Если равен -1, то выделенных прямоугольников не будет
        public static bool timeForTimer;
        public bool sorted; // Флаг, указывающий отсортирован ли массив или нет
        public int selectedSort;
        //private System.Windows.Forms.Timer timer;

        public class myComparer : IComparer<int> // Класс собственного компаратора
        {
            int IComparer<int>.Compare(int x, int y) // Перегружаем метод сравнения. На вход подаются индексы сравниваемых элементов массива
            {
                activeIndex1 = x;
                activeIndex2 = y;
                while (timeForTimer == false) { Thread.Sleep(1); } // Можно вынести в ту же функцию сравнения
                timeForTimer = false; // Можно вынести в ту же функцию сравнения
                return numbers[x]-numbers[y];
            }
        }

        public int callbackCompareFromSortControl(int x, int y, int ix, int iy)
        {
            activeIndex1 = ix;
            activeIndex2 = iy;
            while (timeForTimer == false) { Thread.Sleep(1); } // Можно вынести в ту же функцию сравнения
            timeForTimer = false; // Можно вынести в ту же функцию сравнения
            return x - y;
        }

        public sortControl1() // Конструктор
        {
            timeForTimer = false;
            sorted = false;
            this.DoubleBuffered = true;
            InitializeComponent();
        }

        public void setNumbers(int[] num) // Метод, меняющий и отрисовывающий массив
        {
            numbers = num;
            activeIndex1 = -1;
            activeIndex2 = -1;
            Invalidate();
        }

        public void setActive(int active1, int active2) // Метод, отрисовывающий сравниваемые (активные) элементы массива
        {
            activeIndex1 = active1;
            activeIndex2 = active2;
            Invalidate();
        }

        public void SelectSort() // Выбор алгоритма сортировки
        {
            if (selectedSort == 0)
                Sort<int>.BubbleSort(ref numbers, callbackCompareFromSortControl);
            else if (selectedSort == 1)
                Sort<int>.SelectionSort(ref numbers, callbackCompareFromSortControl);
            else if (selectedSort == 2)
                Sort<int>.InsectionSort(ref numbers, 0, numbers.Length - 1, callbackCompareFromSortControl);
            else
                Sort<int>.QuickSort(ref numbers, 0, numbers.Length - 1, callbackCompareFromSortControl);
        }

        protected override void OnPaint(PaintEventArgs e) // Метод отрисовки окна
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var rect = ClientRectangle; // Получаем размеры области userControl
            g.FillRectangle(brushBack, rect);  // Заполняем задний фон белым

            if (numbers != null)
            {
                int miniRectWidth = (int)Math.Floor((rect.Width) / (double)numbers.Length); // Вычисляем ширину наших прямоугольничков
                int padding = rect.Width - miniRectWidth * numbers.Length;
                for (int i = 0; i < numbers.Length; i++) // Рисуем прямоугольничек для каждого элемента массива
                {
                    int x = miniRectWidth * i+padding/2; // Определяем координату по оси x левого верхнего угла прямоугольничка
                    int y = (int)Math.Round((rect.Height - 2 * 8) * (numbers[i] - 0) / 100.0); // Определяем координату по оси y левого верхнего угла прямоугольничка

                    g.FillRectangle(brushBack, new Rectangle(x, 0, miniRectWidth, rect.Height));
                    var miniRect = new Rectangle(x, rect.Height - y, miniRectWidth - 1, y); // miniRectWidth-1 для красивого разделения прямоугольников
                    if (i == activeIndex1 || i == activeIndex2) // Если данный элемент отмечен, как активный (т.е. который участвует в сравнении)
                        g.FillRectangle(activeRectangle, miniRect); // Закрашиваем его красным
                    else // В противном случае
                        g.FillRectangle(brushRectangle, miniRect); // Закрашиваем серым
                }
            }
        }
    }
}
