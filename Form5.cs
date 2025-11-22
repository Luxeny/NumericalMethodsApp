using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Excel = Microsoft.Office.Interop.Excel;

namespace DehotomiaM
{
    public partial class Form5 : Form
    {
        string[,] list = new string[50, 50];
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = ExportExcel();
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = n + 1;
            dataGridView1.RowCount = n + 1;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                for (int j = 0; j < n; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = list[i, j];
                }//по всем колонкам
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
            int n;
            if (!int.TryParse(textBox1.Text, out n))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            var rand = new Random();
            for (int turn = 0; turn < n; ++turn)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = Convert.ToDouble(rand.Next(-100, 100));
                row.Cells[1].Value = Convert.ToDouble(rand.Next(-100, 100));
                dataGridView1.Rows.Add(row);
            }

        }

        public struct Dots
        {
            public double x;
            public double y;
            public Dots(double myX, double myY)
            {
                x = myX;
                y = myY;
            }
        }
        List<Dots> dots = new List<Dots>();

        double FindMin()
        {
            double min = double.MaxValue;
            for (int turn = 0; turn < dots.Count; ++turn)
            {
                if (dots[turn].x < min) min = dots[turn].x;
            }
            return min;
        }
        double FindMax()
        {
            double max = double.MinValue;
            for (int turn = 0; turn < dots.Count; ++turn)
            {
                if (dots[turn].x > max) max = dots[turn].x;
            }
            return max;
        }

        void DrowDots()
        {
            for (int turn = 0; turn < dots.Count; ++turn)
            {
                this.chart1.Series[0].Points.AddXY(dots[turn].x, dots[turn].y);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            for (int turn = 0; turn < dataGridView1.RowCount; ++turn)
            {
                dots.Add(new Dots(Convert.ToDouble(dataGridView1[0, turn].Value), Convert.ToDouble(dataGridView1[1, turn].Value)));
            }
            DrowDots();
            int n;
            if (!int.TryParse(textBox1.Text, out n))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            double[] X = new double[n];
            double[] Y = new double[n];
            dataGridView1.ColumnCount = n + 1;
            dataGridView1.RowCount = n + 1;
            for (int i = 0; i < n; i++)
            {
                X[i] = Convert.ToInt32(dataGridView1[0, i].Value);
                Y[i] = Convert.ToInt32(dataGridView1[1, i].Value);
            }
            // Создание экземляра класса LSM
            LSM myReg = new LSM(X, Y);

            // Апроксимация заданных значений линейным полиномом
            myReg.Polynomial(1);
            textBox2.AppendText("Линейнная функция: " + "\r\n");
            // Вывод коэффициентов а0 и а1
            textBox2.AppendText($"  A = {myReg.Coeff[1].ToString("F3")}\r\n");
            textBox2.AppendText($"  B = {myReg.Coeff[0].ToString("F3")}\r\n");
            for (int turn = Convert.ToInt32(FindMin()); turn <= Convert.ToInt32(FindMax()); ++turn)
            {
                    double thisY = myReg.Coeff[1] * turn + myReg.Coeff[0];
                    this.chart1.Series[1].Points.AddXY(turn, thisY);
            }
            // Апроксимация заданных значений квадратным полиномом
            myReg.Polynomial(2);
            textBox2.AppendText("\r\n" +"Квадратная функция: " + "\r\n");
            // Вывод коэффициентов а0, а1 и а2
            textBox2.AppendText($"  A = {myReg.Coeff[2].ToString("F3")}\r\n");
            textBox2.AppendText($"  B = {myReg.Coeff[1].ToString("F3")}\r\n");
            textBox2.AppendText($"  C = {myReg.Coeff[0].ToString("F3")}\r\n");
            for (int turn = Convert.ToInt32(FindMin()); turn <= Convert.ToInt32(FindMax()); ++turn)
            {
                double thisY = myReg.Coeff[2] * turn * turn + myReg.Coeff[1] * turn + myReg.Coeff[0];
                this.chart1.Series[2].Points.AddXY(turn, thisY);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dots.Clear();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
