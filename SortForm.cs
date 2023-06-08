using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace VisualSort
{
    public partial class SortForm : Form
    {

        public SortForm() // Конструктор формы
        {
            InitializeComponent();
            comboBox1.Items.Add("Сортировка пузырьком");
            comboBox1.Items.Add("Сортировка выбором");
            comboBox1.Items.Add("Сортировка вставками");
            comboBox1.Items.Add("Быстрая сортировка");
            comboBox1.SelectedIndex = 0;
            label2.Text = "Количество элементов в массиве (" + trackBar1.Value + "):";
            label3.Text = "Скорость таймера (" + Math.Round((double)trackBar2.Value / 10.0)/100.0 + " секунд):";
            timer1.Interval = trackBar2.Value;
        }

        private void button1_Click(object sender, EventArgs e) // Обработка кнопки "Сгенерировать массив" - генерируем массив случайных чисел и выводим его на экран
        {
            int[] numbers = new int[trackBar1.Value]; // Создаем массив из нужного числа элементов
            Random rand = new Random(); // Заполняем его случайными числами
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = rand.Next(0,100);

            sortControl1.setNumbers(numbers); // Сохраняем массив внутри sortControl1 и отрисовываем его
        }

        private void button3_Click(object sender, EventArgs e) // Обработка кнопки "Очистить"
        {
            int[] numbers = null; // Удаляем массив
            sortControl1.setNumbers(numbers); // Отрисовываем пустое окно
        }

        private void button2_Click(object sender, EventArgs e) // Обработка клика по кнопку "Сортировать"
        {
            // Делаем элементы управления неактивными на момент сортировки
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            trackBar1.Enabled = false;
            comboBox1.Enabled = false;

            timer1.Start(); // Запускаем таймер
            Thread thread1 = new Thread(threadMethod); // Создаем поток с вызовом функции сортировки
            thread1.Start(); // Запускаем поток (будет выполняться до тех пор, пока выполняется функция сортировки)
        }

        public void threadMethod() // Создаем метод, который будет являться стартовой точкой для потока сортировки
        {
            sortControl1.sorted = false; // Если была вызвана сортировка, то полагаем, что массив еще не отсортирован и надо прорисовать анимацию сортировки
            sortControl1.SelectSort(); // Вызываем функцию сортировки
            sortControl1.setActive(-1, -1); // После сортировки еще раз перерисовываем массив, чтобы убрать красные прямоугольнички и поменять последние неотсортированные элементы местами
            sortControl1.sorted = true; // Массив отсортирован
        }

        private void timer1_Tick(object sender, EventArgs e) // Обработчик таймера
        {
            if(sortControl1.sorted == false) // Если массив не отсортирован
            {
                sortControl1.timeForTimer = true; // Сообщаем потоку с сортировкой, что наступило время перерисовки и можно прокрутить сортировку до следующего сравнения
                sortControl1.setActive(sortControl1.activeIndex1, sortControl1.activeIndex2); // Вызываем перерисовку
            } else // Если массив уже отсортирован
            {
                // Делаем активными все элементы управления
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                trackBar1.Enabled = true;
                comboBox1.Enabled = true;
                timer1.Stop(); // Останавливаем таймер
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) // Обработка трекбара с количеством элементов в массиве
        {
            label2.Text = "Количество элементов в массиве (" + trackBar1.Value + "):"; // Выводим текущее количество элементов в массиве
        }

        private void trackBar2_Scroll(object sender, EventArgs e) // Обработка трекбара с задержкой между перерисовками
        {
            label3.Text = "Скорость таймера (" + Math.Round((double)trackBar2.Value / 10.0) / 100.0 + " секунд):"; // Выводим текущую задержку между перерисовками в секундах
            timer1.Interval = trackBar2.Value; // Назначаем новый интервал таймеру
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Обработка изменения сортировки
        {
            sortControl1.selectedSort = comboBox1.SelectedIndex;
        }
    }
}
