using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DehotomiaM
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(textBoxF.Text.ToLower());
            expression.Bind("x", X);
            return expression.Eval<double>();
        }
        private void AddVerticalLine(ChartArea chartArea, double position, Color color)
        {
            VerticalLineAnnotation verticalLine = new VerticalLineAnnotation();
            verticalLine.AxisX = chartArea.AxisX;
            verticalLine.AxisY = chartArea.AxisY;
            verticalLine.LineColor = color;
            verticalLine.LineWidth = 2; // Adjust the line width as needed
            verticalLine.IsInfinitive = true;
            verticalLine.ClipToChartArea = chartArea.Name;
            verticalLine.AnchorX = position;

            chart1.Annotations.Add(verticalLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double a, b, Xi;
            int n;
            string text = "";
            if (!double.TryParse(textBoxA.Text, out a) || !double.TryParse(textBoxB.Text, out b))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            if (a >= b)
            {
                throw new ArgumentException("Некорректные границы интервала");
            }
            bool hasE = double.TryParse(textBoxE.Text, out Xi);
            bool hasN = int.TryParse(textBoxN.Text, out n);

            if (!hasE && !hasN)
            {
                throw new ArgumentException("Укажите точность (E) ИЛИ количество разбиений (N)");
            }

            this.chart1.Series[0].Points.Clear();
            double x = a;
            double y;
            while (x <= b)
            {
                y = F(x);
                this.chart1.Series[0].Points.AddXY(x, y);
                x += 0.1;
            }
            AddVerticalLine(chart1.ChartAreas[0], a, Color.Green);
            AddVerticalLine(chart1.ChartAreas[0], b, Color.Green);
            this.chart1.Series[0].Color = Color.Green;
            this.chart1.Series[0].BorderWidth = 2;
            if (!string.IsNullOrEmpty(textBoxN.Text))
            {
                if (checkBox1.Checked)
                {
                    int numSteps = int.Parse(textBoxN.Text);
                    FMethod method = new FMethod();
                    double result = method.LeftRectangle(a, b, numSteps, F);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Прямоугольника: {resultAsDecimal:F2}" + "\r\n\r\n";
                    //textBoxN.Text = $"{resultAsDecimal:F5}";
                }
                if (checkBox2.Checked)
                {
                    int numSteps = int.Parse(textBoxN.Text);
                    FMethod method1 = new FMethod();
                    double result = method1.Simpson(a, b, numSteps, F);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Симпсона: {resultAsDecimal:F2}" + "\r\n\r\n";
                    // textBoxN.Text = $"{resultAsDecimal:F5}";
                }
                if (checkBox3.Checked)
                {
                    int numSteps = int.Parse(textBoxN.Text);
                    FMethod method2 = new FMethod();
                    double result = method2.Trapezoidal(a, b, numSteps, F);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Трапеции: {resultAsDecimal:F2}" + "\r\n\r\n";
                    // textBoxN.Text = $"{resultAsDecimal:F5}";
                }
            }
            if (!string.IsNullOrEmpty(textBoxE.Text))
            {
                if (checkBox1.Checked)
                {
                    FMethod method = new FMethod();
                    double result = method.RectangleMethod(F,a, b, Xi, out int Opt);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Прямоугольника: {resultAsDecimal:F2}" + "\r\n\r\n";
                    //textBoxN.Text = $"{resultAsDecimal:F5}";
                }
                if (checkBox2.Checked)
                {
                    FMethod method1 = new FMethod();
                    double result = method1.SimpsonMethod(F, a, b, Xi, out int Opt);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Симпсона: {resultAsDecimal:F2}" + "\r\n\r\n";
                    // textBoxN.Text = $"{resultAsDecimal:F5}";
                }
                if (checkBox3.Checked)
                {
                    FMethod method2 = new FMethod();
                    double result = method2.TrapezoidalMethod(F, a, b, Xi, out int Opt);
                    decimal resultAsDecimal = Convert.ToDecimal(result);
                    text += $"Количество разбиений Методом Трапеции: {resultAsDecimal:F2}" + "\r\n\r\n";
                    // textBoxN.Text = $"{resultAsDecimal:F5}";
                }
            }
            textBox4.Text = text;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
