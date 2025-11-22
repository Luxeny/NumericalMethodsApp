using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehotomiaM
{
    internal class FMethod
    {
        public double LeftRectangle(double A, double B, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;

            for (int i = 0; i <= N - 1; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        private double RightRectangle(double A, double B, double E, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;

            for (var i = 1; i <= N; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        private double CentralRectangle(double A, double B, double E, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = (F(A) + F(B)) / 2;
            double x = 0d;
            double y = 0d;

            for (var i = 1; i < N; i++)
            {
                x = A + h * i;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        public double Simpson(double A, double B, double N, Func<double, double> F)
        {

            double h = (B - A) / N;
            double sum1 = 0d;
            double sum2 = 0d;
            double xk = 0d, yk = 0d, xk_1 = 0d;
            for (double k = 1; k <= N; k++)
            {
                xk = A + (k * h);
                if (k <= N - 1)
                {
                    yk = F(xk);
                    sum1 += yk;
                }
                xk_1 = A + ((k - 1) * h);
                sum2 += F((xk + xk_1) / 2);
            }
            double result = h / 3d * ((1d / 2d * F(A)) + sum1 + (2 * sum2) + (1d / 2d * F(B)));
            return result;
        }
        public double Trapezoidal(double A, double B, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double xk = 0d, yk = 0d;
            for (double k = 1; k <= N - 2; k++)
            {
                xk = A + (k * h);
                yk = F(xk);
                sum += yk;
            }
            xk = A + ((N - 1) * h);

            return h / 2d * (F(A) + F(xk)) + (h * sum);
        }
        public double RectangleMethod(Func<double, double> func, double a, double b, double exp, out int Opt)
        {
            int n = 1; // Начальное количество разбиений
            double h = (b - a) / n; // Шаг разбиения
            double integral = 0.0;
            double previousIntegral = double.MaxValue;

            while (Math.Abs(previousIntegral - integral) > exp)
            {
                previousIntegral = integral;
                integral = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double x_i = a + i * h; //double x_i = a + i * h + h / 2.0; 
                    integral += h * func(x_i); // Площадь текущего прямоугольника
                }

                n *= 2; // Удвоение числа разбиений
                h = (b - a) / n; // Пересчет шага
            }
            Opt = n / 2;

            return integral;
        }
        public  double SimpsonMethod(Func<double, double> func, double a, double b, double exp, out int Opt)
        {
            int n = 1; // Начальное количество разбиений
            double h = (b - a) / n; // Шаг разбиения
            double integral = 0.0;
            double previousIntegral = double.MaxValue;

            while (Math.Abs(previousIntegral - integral) > exp)
            {
                previousIntegral = integral;
                integral = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double x_i = a + i * h; // Левая граница текущего интервала
                    double x_next = a + (i + 1) * h; // Правая граница текущего интервала
                    double x_mid = (x_i + x_next) / 2.0; // Середина текущего интервала

                    integral += h / 6.0 * (func(x_i) + 4 * func(x_mid) + func(x_next)); // Площадь интервала по методу Симпсона
                }

                n *= 2; // Удвоение числа разбиений
                h = (b - a) / n; // Пересчет шага
            }

            Opt = n / 2;

            return integral;
        }


        public double TrapezoidalMethod(Func<double, double> func, double a, double b, double exp, out int Opt)
        {
            int n = 1; // Начальное количество разбиений
            double h = (b - a) / n; // Шаг разбиения
            double integral = 0.0;
            double previousIntegral = double.MaxValue;

            while (Math.Abs(previousIntegral - integral) > exp)
            {
                previousIntegral = integral;
                integral = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double x_i = a + i * h; // Левая граница текущей трапеции
                    double x_next = a + (i + 1) * h; // Правая граница текущей трапеции

                    integral += h * (func(x_i) + func(x_next)) / 2.0; // Площадь текущей трапеции
                }

                n *= 2; // Удвоение числа разбиений
                h = (b - a) / n; // Пересчет шага
            }
            Opt = n / 2;

            return integral;
        }
    }
}
