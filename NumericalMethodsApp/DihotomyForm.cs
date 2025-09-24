using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
                string expr = txtFunction.Text.Replace("x", x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                DataTable dt = new DataTable();
                var result = dt.Compute(expr, "");
                return Convert.ToDouble(result);
            }
            catch
            {
                MessageBox.Show("Ошибка в формуле!");
                return 0;
            }
        }

        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double a = double.Parse(txtA.Text);
                double b = double.Parse(txtB.Text);
                double eps = double.Parse(txtE.Text);

                double delta = eps / 2;
                double x1, x2;

                while (Math.Abs(b - a) > eps)
                {
                    x1 = (a + b - delta) / 2;
                    x2 = (a + b + delta) / 2;

                    if (EvaluateFunction(x1) < EvaluateFunction(x2))
                        b = x2;
                    else
                        a = x1;
                }

                double xmin = (a + b) / 2;
                double fmin = EvaluateFunction(xmin);

                lblResult.Text = $"Минимум: x = {xmin:F4}, f(x) = {fmin:F4}";

                chart1.Series[0].Points.Clear();
                for (double x = a - 2; x <= b + 2; x += 0.1)
                    chart1.Series[0].Points.AddXY(x, EvaluateFunction(x));

                chart1.Series[1].Points.Clear();
                chart1.Series[1].Points.AddXY(xmin, fmin);
            }
            catch
            {
                MessageBox.Show("Ошибка ввода!");
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
    }
}