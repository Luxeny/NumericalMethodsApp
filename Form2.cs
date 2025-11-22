using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Factorization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DehotomiaM
{
    public partial class Form2 : Form
    {
        string[,] list = new string[50, 50];
        public Form2()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Число";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = ExportExcel();
            dataGridView1.Rows.Clear();
            string s;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < 50; j++) //по всем колонкам
                    s += list[i, j];
                dataGridView1.Rows.Add(s);
            }

        }
        private int ExportExcel()
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xls;*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл базы данных";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
                return 0;
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];//получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
                                                                                                // размеры базы
            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;
            // Перенос в промежуточный массив класса Form1: string[,] list = new string[50, 5]; 
            for (int j = 0; j < 5; j++) //по всем колонкам
                for (int i = 0; i < lastRow; i++) // по всем строкам
                    list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString(); //считываем данные
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
            return lastRow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateRandomData();
        }
        private void GenerateRandomData()
        {
            dataGridView1.Rows.Clear();
            double a;
            if (!double.TryParse(textBox2.Text, out a))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            var RandomNumber = new Random((int)Stopwatch.GetTimestamp());
            for (int i = 0; i < a; i++)
            {
                double number;
                number = RandomNumber.Next(-10000, 10000) - 10 + 15;
                dataGridView1.Rows.Add(number);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SortNumbers(SortOrder.Ascending);
        }

        public struct SortStats
        {
            public double Time { get; set; }
            public int Iterations { get; set; }

        }
        public void SortNumbers(SortOrder sortOrder)
        {  // выбрана ли хотя бы однасортировка?
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked &&
                !checkBox4.Checked && !checkBox5.Checked)
            {
                MessageBox.Show("Отсутствуют данные для сортировки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }
            Dictionary<string, SortStats> sortStats = new Dictionary<string, SortStats>();
            bool ascendingKey = true;

            if (checkBox2.Checked)
            {
                sortStats["Быстрая"] = MeasureSortingStats(() => QuickSort(dataGridViewNumbers, 0, dataGridViewNumbers.Count - 1, sortOrder, ascendingKey));
            }
            if (checkBox3.Checked)
            {
                sortStats["Шейкерная"] = MeasureSortingStats(() => ShakerSort(dataGridViewNumbers, sortOrder));
            }
            if (checkBox1.Checked)
            {
                sortStats["Пузырьковая"] = MeasureSortingStats(() => BubbleSort(dataGridViewNumbers, sortOrder));
            }
            if (checkBox5.Checked)
            {
                sortStats["Вставками"] = MeasureSortingStats(() => InsertionSort(dataGridViewNumbers, sortOrder, ascendingKey));
            }
            if (checkBox4.Checked)
            {
                sortStats["BOGO"] = MeasureSortingStats(() => BogoSort(dataGridViewNumbers, sortOrder));
            }
            textBox1.Clear();
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var kvp in sortStats)
            {
                resultBuilder.AppendLine($"{kvp.Key}: \r\nВремя выполнения - {kvp.Value.Time} нс, Количество итераций - {kvp.Value.Iterations}");
            }
            string text2 = "";
            for (int i = 0; i < dataGridViewNumbers.Count; i++)
            {
                text2 += dataGridViewNumbers[i].ToString() + " ";
            }
            textBox1.Text = "Отсортированный массив :" + "\r\n\r\n" + text2;// + "\r\n\r\n" + "Результаты сортировок: " + resultBuilder;
            textBox3.Text = "Результаты сортировок: " + resultBuilder;
        }
        // int count = 0;
        private SortStats MeasureSortingStats(Action sortingAction)
        {
            //count = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            sortingAction();
            stopwatch.Stop();
            double time = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000000; //получает общее затраченное время и делим на частоту тактов в секунду и умножаем на 
            return new SortStats { Time = time, Iterations = count };
        }
        
        private void UpdateChart(List<double> list)
        {
            chart1.Series.Clear();
            chart1.Series.Add("Numbers");

            foreach (var number in list)
            {
                chart1.Series["Numbers"].Points.AddY(number);
            }

            chart1.Invalidate();
        }
        int count;
        void BubbleSort(List<double> list, SortOrder sortOrder)
        {
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }
            count = 0;
            int n = dataGridViewNumbers.Count;
            double temp;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                   
                    if ((sortOrder == SortOrder.Ascending && dataGridViewNumbers[j] > dataGridViewNumbers[j + 1]) ||
                        (sortOrder == SortOrder.Descending && dataGridViewNumbers[j] < dataGridViewNumbers[j + 1]))
                    {
                        temp = dataGridViewNumbers[j];
                        dataGridViewNumbers[j] = dataGridViewNumbers[j + 1];
                        dataGridViewNumbers[j + 1] = temp;

                       // UpdateChart(list);
                       count++;
                    }
                    
                }
            }
            UpdateChart(list);
        }
        private string Insertion = string.Empty;
        //count = 0;
        void InsertionSort(List<double> list, SortOrder sortOrder,bool ascending)
        {
            // начинаем со второго элемента (элемент с индексом 0
            // уже отсортировано)
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }
            count = 0;
            double n = dataGridViewNumbers.Count;
            for (int i = 1; i < n; i++)
            {
                double k = dataGridViewNumbers[i];
                int j = i - 1;
               // count++;
                while (j >= 0 && ((ascending && list[j] > k) || (!ascending && dataGridViewNumbers[j] < k)))
                {
                    count++;
                    dataGridViewNumbers[j + 1] = dataGridViewNumbers[j];
                    dataGridViewNumbers[j] = k;
                    j--;
                    // UpdateChart(list);
                   
                }
                dataGridViewNumbers[j + 1] = k;
            }
            Insertion = "Итерации Вставками  " + count.ToString();
            UpdateChart(list);
        }

        private void QuickSort(List<double> list, int left, int right, SortOrder sortOrder, bool ascending)
        {
             count = 0;
            if (left < right)
            {
                int pivot = Partition(list, left, right, sortOrder, ascending);

                QuickSort(list, left, pivot - 1, sortOrder, ascending);
                QuickSort(list, pivot + 1, right, sortOrder, ascending);
                count++;
            }
            UpdateChart(list);
            //count++;

        }


        //Функция для нахождения основного элемена
         int Partition(List<double> list, int left, int right, SortOrder sortOrder, bool ascending)
        {
            count = 0;
            double pivot = list[right];
            int i = (left - 1);

            for (int j = left; j < right; j++)
            {
                count++;
                if ((ascending && list[j] <= pivot) || (!ascending && list[j] >= pivot))
                {
                    i++;
                    double temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
            double temp1 = list[i + 1];
            list[i + 1] = list[right];
            list[right] = temp1;
            return i + 1;
        }

        private string Shaker = string.Empty;
        void ShakerSort(List<double> list, SortOrder sortOrder)
        {
            count = 0;
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }

            int left = 0;
            int right = dataGridViewNumbers.Count - 1;
            bool swapped = true;
            int count1 = 0;
            int count2 = 0;
            while (left < right && swapped)
            {
              // count++;
                swapped = false;
                for (int i = left; i < right; ++i)
                {
                    if ((sortOrder == SortOrder.Ascending && dataGridViewNumbers[i] > dataGridViewNumbers[i + 1]) ||
                        (sortOrder == SortOrder.Descending && dataGridViewNumbers[i] > dataGridViewNumbers[i + 1]))
                    {
                        double temp = dataGridViewNumbers[i];
                        dataGridViewNumbers[i] = dataGridViewNumbers[i + 1];
                        dataGridViewNumbers[i + 1] = temp;
                        swapped = true;
                    }
                    count1++;
                }
                --right;
                for (int i = right; i > left; --i)
                {
                    if ((sortOrder == SortOrder.Descending && dataGridViewNumbers[i] < dataGridViewNumbers[i - 1]) ||
                        (sortOrder == SortOrder.Ascending && dataGridViewNumbers[i] < dataGridViewNumbers[i - 1]))
                    {
                        double temp = dataGridViewNumbers[i];
                        dataGridViewNumbers[i] = list[i - 1];
                        dataGridViewNumbers[i - 1] = temp;
                     //   Swap(list, i, i - 1);
                        swapped = true;
                    }
                    count2++;
                }
                ++left;
                UpdateChart(list);
                count = count1 + count2;
            }
        }
        
        void BogoSort(List<double> list, SortOrder sortOrder)
        {
            count = 0;
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }
            Random random = new Random();
            //Проверка упорядоченности массива
            while (!IsSorted(dataGridViewNumbers, sortOrder))
            {
               // count++;
                Shuffle(dataGridViewNumbers, random);
                count++;
            }
            UpdateChart(list);

        }
        //Встряхиваем рандомно все значения, в надежде,что все значения встанут правильно. 

        void Shuffle(List<double> list, Random random)
         {
             int n = list.Count;
           //  Random random = new Random();
             while (n > 1)
             {
                 --n;
                 int randomIndex = random.Next(n + 1);
                 double temp = list[randomIndex];
                 list[randomIndex] = list[n];
                 list[n] = temp;
             }
         }


        //Проверяем отсортирован ли массив? 
        static bool IsSorted(List<double> list, SortOrder sortOrder)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private void deledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
