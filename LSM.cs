using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehotomiaM
{
    internal class LSM
    {
        // Массивы значений Х и У задаются как свойства
        public double[] X { get; set; }
        public double[] Y { get; set; }

        // Искомые коэффициенты полинома в данном случае, а в общем коэфф. при функциях
        private double[] coeff;
        public double[] Coeff { get { return coeff; } }

        // Конструктор класса. Примает 2 массива значений х и у
        // Длина массивов должна быть одинакова, иначе нужно обработать исключение
        public LSM(double[] x, double[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException("X and Y arrays should be equal!");
            X = new double[x.Length];
            Y = new double[y.Length];

            for (int i = 0; i < x.Length; i++)
            {
                X[i] = x[i];
                Y[i] = y[i];
            }
        }

        // Собственно, Метод Наименьших Квадратов
        // В качестве базисных функций используются степенные функции y = a0 * x^0 + a1 * x^1 + ... + am * x^m
        public void Polynomial(int m)
        {

            if (m <= 0) throw new ArgumentException("Порядок полинома должен быть больше 0");
            if (m >= X.Length) throw new ArgumentException("Порядок полинома должен быть на много меньше количества точек!");

            // массив для хранения значений базисных функций
            double[,] basic = new double[X.Length, m + 1];

            // заполнение массива для базисных функций
            for (int i = 0; i < basic.GetLength(0); i++)
                for (int j = 0; j < basic.GetLength(1); j++)
                    basic[i, j] = Math.Pow(X[i], j);

            // Создание матрицы из массива значений базисных функций(МЗБФ)
            Matrix basicFuncMatr = new Matrix(basic);

            // Транспонирование МЗБФ
            Matrix transBasicFuncMatr = basicFuncMatr.Transposition();

            // Произведение транспонированного  МЗБФ на МЗБФ
            Matrix lambda = transBasicFuncMatr * basicFuncMatr;

            // Произведение транспонированого МЗБФ на следящую матрицу 
            Matrix beta = transBasicFuncMatr * new Matrix(Y);

            // Решение СЛАУ путем умножения обратной матрицы лямбда на бету
            Matrix a = lambda.InverseMatrix() * beta;

            // Присвоение значения полю класса 
            coeff = new double[a.Row];
            for (int i = 0; i < coeff.Length; i++)
            {
                coeff[i] = a.Args[i, 0];
            }
        }
    }
}
