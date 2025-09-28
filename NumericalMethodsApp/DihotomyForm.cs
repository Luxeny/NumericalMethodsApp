using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace DichotomyApp
{
    public partial class DihotomyForm : Form
    {
        public DihotomyForm()
        {
            InitializeComponent();
        }

        private double EvaluateFunction(double x)
        {
            try
            {
                string expr = txtFunction.Text
                    .Replace("x", x.ToString(CultureInfo.InvariantCulture))
                    .Replace(" ", "");
                
                expr = expr.Replace("sin", "Math.Sin")
                          .Replace("cos", "Math.Cos")
                          .Replace("tan", "Math.Tan")
                          .Replace("atan", "Math.Atan")
                          .Replace("exp", "Math.Exp")
                          .Replace("log", "Math.Log")
                          .Replace("sqrt", "Math.Sqrt")
                          .Replace("abs", "Math.Abs")
                          .Replace("pow", "Math.Pow");
                
                if (expr.Contains("^"))
                {
                    var parts = expr.Split('^');
                    if (parts.Length == 2)
                    {
                        expr = $"Math.Pow({parts[0]},{parts[1]})";
                    }
                }

                DataTable dt = new DataTable();
                var result = dt.Compute(expr, "");
                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в формуле: {ex.Message}");
                return double.NaN;
            }
        }

        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double a = double.Parse(txtA.Text);
                double b = double.Parse(txtB.Text);
                double eps = double.Parse(txtE.Text);

                double fa = EvaluateFunction(a);
                double fb = EvaluateFunction(b);
                
                if (double.IsNaN(fa) || double.IsNaN(fb))
                    return;
                
                if (fa * fb >= 0)
                {
                    MessageBox.Show("Функция должна иметь разные знаки на концах интервала [a,b]!");
                    return;
                }

                double x0 = 0;
                int iterations = 0;
                const int maxIterations = 1000;

                while (Math.Abs(b - a) > eps && iterations < maxIterations)
                {
                    x0 = (a + b) / 2;
                    double fx0 = EvaluateFunction(x0);

                    if (double.IsNaN(fx0))
                        return;

                    if (Math.Abs(fx0) < eps)
                        break;

                    if (fa * fx0 < 0)
                        b = x0;
                    else
                    {
                        a = x0;
                        fa = fx0;
                    }

                    iterations++;
                }

                if (iterations >= maxIterations)
                {
                    MessageBox.Show("Достигнуто максимальное количество итераций!");
                }

                double root = (a + b) / 2;
                double froot = EvaluateFunction(root);

                if (double.IsNaN(froot))
                    return;

                lblResult.Text = $"Корень: x = {root:F6}, f(x) = {froot:E6}";

                chart1.Series[0].Points.Clear();
                double step = (b - a + 4) / 100;
                for (double x = a - 2; x <= b + 2; x += step)
                {
                    double y = EvaluateFunction(x);
                    if (!double.IsNaN(y) && !double.IsInfinity(y))
                        chart1.Series[0].Points.AddXY(x, y);
                }

                chart1.Series[1].Points.Clear();
                chart1.Series[1].Points.AddXY(root, froot);
                
                chart1.Series[1].Points.AddXY(root, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtA.Clear();
            txtB.Clear();
            txtE.Clear();
            txtFunction.Clear();
            lblResult.Text = "";
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
        }

        private void DihotomyForm_Load(object sender, EventArgs e)
        {
            txtFunction.Text = "sin(x)";
            txtA.Text = "1";
            txtB.Text = "3";
            txtE.Text = "0.0001";
        }
    }
}
