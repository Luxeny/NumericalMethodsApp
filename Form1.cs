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
using org.matheval;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Differentiation;
using System.Net;
using MathNet.Numerics.Differentiation;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;

namespace DehotomiaM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(textBox7.Text.ToLower());
            expression.Bind("x", X);
            decimal value = expression.Eval<decimal>();
            return (double)value;
        }
        double Func(double X)
        {
            return -F(X);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
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
                Xi = (int)-Math.Log10(Xi);
                double Root;
                if (F(a) * F(b) <= 0)
                {
                    MessageBox.Show("Условие сходимости выполнено");
                    Root = (a + b) / 2;

                    while (Math.Abs(b - a) > Math.Pow(10, -Xi))
                    {
                        double y1 = F(a), y2 = F(b), y3 = F(Root);
                        if (y1 * y3 < 0)
                        {
                            b = Root;
                        }
                        else if (y2 * y3 < 0)
                        {
                            a = Root;
                        }
                        else
                        {
                            break;
                        }
                        Root = (a + b) / 2;

                    }
                    if ((27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root / 3) < 0 + Root && (27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root / 3) > 0 - Root)
                    {
                        textBox4.Text = Root.ToString(); ;
                    }
                }
                else
                {
                    throw new ArgumentException("Нет корней на этом интервале или их больше одного");
                }
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                double max, min;
                double delta = Xi / 10;
                while (b - a >= Xi)
                {
                    double middle = (a + b) / 2;
                    double lambda = middle - delta, mu = middle + delta;
                    if (F(lambda) < F(mu))
                        b = mu;
                    else
                        a = lambda;
                }
                min = (a + b) / 2;


                // Точка максимума
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                while (b - a >= Xi)
                {
                    double middle = (a + b) / 2;
                    double lambda = middle - delta, mu = middle + delta;
                    if (F(lambda) > F(mu))
                        b = mu;
                    else
                        a = lambda;
                }
                max = (a + b) / 2;

                textBox5.Text = min.ToString();
                textBox6.Text = max.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        public void GoldenSectionSearchMin(Func<double, double> f, double StartPoint, double EndPoint, double epsilon)
        {
            double x1, x2, k1, k2, F1, F2, Result;
            int count = 0;
            k2 = (Math.Sqrt(5) - 1) / 2;
            k1 = 1 - k2;
            x1 = StartPoint + k1 * (EndPoint - StartPoint);
            x2 = StartPoint + k2 * (EndPoint - StartPoint);
            try
            {
                F1 = F(x1);
                F2 = F(x2);
                while (true)
                {
                    ++count;
                    if ((EndPoint - StartPoint) < epsilon)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        textBox5.Text = Result.ToString();
                        break;
                    }
                    else
                    {
                        if (F1 < F2)
                        {
                            EndPoint = x2;
                            x2 = x1;
                            F2 = F1;
                            x1 = StartPoint + k1 * (EndPoint - StartPoint);
                            F1 = F(x1);
                        }
                        else
                        {
                            StartPoint = x1;
                            x1 = x2;
                            F2 = F1;
                            x2 = StartPoint + k2 * (EndPoint - StartPoint);
                            F2 = F(x2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void GoldenSectionSearchMax(Func<double, double> f, double StartPoint, double EndPoint, double epsilon)
        {
            double x1, x2, k1, k2, F1, F2, Result;
            int count = 0;
            k2 = (Math.Sqrt(5) - 1) / 2;
            k1 = 1 - k2;
            x1 = StartPoint + k1 * (EndPoint - StartPoint);
            x2 = StartPoint + k2 * (EndPoint - StartPoint);
            try
            {
                F1 = F(x1);
                F2 = F(x2);
                while (true)
                {
                    ++count;
                    if ((EndPoint - StartPoint) < epsilon)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        textBox6.Text = Result.ToString();
                        break;
                    }
                    else
                    {
                        if (F1 > F2)
                        {
                            EndPoint = x2;
                            x2 = x1;
                            F2 = F1;
                            x1 = StartPoint + k1 * (EndPoint - StartPoint);
                            F1 = F(x1);
                        }
                        else
                        {
                            StartPoint = x1;
                            x1 = x2;
                            F2 = F1;
                            x2 = StartPoint + k2 * (EndPoint - StartPoint);
                            F2 = F(x2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public double MethodOfNewton(double a, double b, double accuracy)
        {

            double x0;

            x0 = (a + b) / 2;

            double x1 = x0;

            do
            {
                x0 = x1;
                x1 = x0 - (F(x0) / Differentiate.FirstDerivative(F,x0));
            } while (Math.Abs(F(x0) / Differentiate.FirstDerivative(F, x0)) >= accuracy);

            return x1;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
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

                GoldenSectionSearchMax(F, a, b, Xi);
                GoldenSectionSearchMin(F, a, b, Xi);
                double root = MethodOfNewton(a, b, Xi);
                textBox4.Text = root.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

        }
        Form2 F2;
        private void сортировкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F2 = new Form2();
            F2.Show();

        }
        Form3 F3;

        /* void DescentMethodRoot(double a, double b, double epsilon)
         {

             double x = (a + b) / 2; // Начальное приближение

             while (Math.Abs(F(x)) > epsilon)
             {
                 // Обновление координаты x по очереди
                 for (int i = 0; i < 100; i++) // Максимальное количество итераций
                 {
                     double prevX = x;
                     x = a + (F(b) * (b - a)) / (F(b) - F(a));
                     if (Math.Abs(F(x)) <= epsilon || Math.Abs(x - prevX) <= epsilon)
                         break;
                     if (F(x) * F(a) < 0)
                         b = x;
                     else
                         a = x;
                 }
                 if (F(x) * F(a) < 0)
                     b = x;
                 else
                     a = x;
             }


             textBox4.Text = x.ToString();

         }*/
        public double CoordinateDescentMin(double interval1, double interval2, int accuracy)
        {
            double a = interval1, b = interval2;
            double x = (a + b) / 2; // Инициализация начального значения x
            double delta = 1 / Math.Pow(10, accuracy); // Вычисление дельты по точности n

            while (b - a > delta) // Условие остановки
            {
                if (F(a) > F(b))
                    a = x; // Если значение функции в точке a больше, обновляем начальную границу
                else
                    b = x; // Иначе обновляем конечную границу

                x = (a + b) / 2; // Вычисление нового значения x
            }

            return Math.Round(x, accuracy); // Возвращаем значение x с заданной точностью n
        }
        public double AntiCoordinateDescent(double interval1, double interval2, int accuracy)
        {
            double a = interval1, b = interval2;
            double x = (a + b) / 2; // Инициализация начального значения x
            double delta = 1 / Math.Pow(10, accuracy); // Вычисление дельты по точности n

            while (b - a > delta) // Условие остановки
            {
                if (-F(a) > -F(b))
                    a = x; // Если значение функции в точке a больше, обновляем начальную границу
                else
                    b = x; // Иначе обновляем конечную границу

                x = (a + b) / 2; // Вычисление нового значения x
            }

            return Math.Round(x, accuracy); // Возвращаем значение x с заданной точностью n
        }

        public void button3_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi, n;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
                }
                this.chart1.Series[0].Points.Clear();
               // Dictionary <double, double>  f1 = new Dictionary<double, double>();
                this.chart1.Series[0].ChartType = SeriesChartType.Line;
                this.chart1.Series[0].MarkerStyle = MarkerStyle.Circle;


                double y;
                double c = 1;
                for (double x = a; x <= b; x +=0.1)
                {
                    y = F(x);
                    if (x == 0)
                        y = 0;
                    this.chart1.Series[0].Points.AddXY(x, y);

                  //  y = F(x);
                  //  this.chart1.Series[0].Points.AddXY(x,y);
                  //  x += 0.1;
                }
                this.chart1.Series[0].Color = Color.Green;
                this.chart1.Series[0].BorderWidth = 2;
                //  DescentMethodRoot(a, b, Xi);

                double resultMin = CoordinateDescentMin(a, b, (int)-Math.Log10(Xi));
                double resultMax = AntiCoordinateDescent(a, b, (int)-Math.Log10(Xi));
                textBox5.Text = resultMin.ToString();
                textBox6.Text = resultMax.ToString();
                if (resultMin == a || resultMin == b)
                {
                    throw new ArgumentException("Точки минимум нет на данном интервале");
                }
                else
                {
                    textBox5.Text = resultMin.ToString();
                }
                if (resultMax == a || resultMax == b)
                {
                    throw new ArgumentException("Точки максимум нет на данном интервале");  
                }
                else
                {
                    textBox6.Text = resultMax.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }


        }

        private void интегралыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F3 = new Form3();
            F3.Show();
        }
        Form4 F4;

        private void сЛАУToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F4 = new Form4();
            F4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            this.chart1.Series[0].Points.Clear();

        }
        Form5 F5;

        private void наименьшийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F5 = new Form5();
            F5.Show();
        }
    }
}
