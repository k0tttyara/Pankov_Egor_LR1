using System;
using WpfApp2.Services.Interfaces;

namespace WpfApp2.Services
{
    public class CalculatorService : ICalculatorService
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;

        public double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Деление на ноль");
            return a / b;
        }

        public double Power(double a, double b) => Math.Pow(a, b);

        public double SquareRoot(double a)
        {
            if (a < 0) throw new ArgumentException("Корень из отрицательного числа");
            return Math.Sqrt(a);
        }

        public double Square(double a) => a * a;

        public double Sin(double degrees) => Math.Sin(degrees * Math.PI / 180.0);
        public double Cos(double degrees) => Math.Cos(degrees * Math.PI / 180.0);
        public double Tan(double degrees) => Math.Tan(degrees * Math.PI / 180.0);

        public double Log10(double a)
        {
            if (a <= 0) throw new ArgumentException("log от неположительного числа");
            return Math.Log10(a);
        }

        public double Ln(double a)
        {
            if (a <= 0) throw new ArgumentException("ln от неположительного числа");
            return Math.Log(a);
        }
    }
}