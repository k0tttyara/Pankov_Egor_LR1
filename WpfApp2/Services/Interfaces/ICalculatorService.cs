namespace WpfApp2.Services.Interfaces
{
    public interface ICalculatorService
    {
        double Add(double a, double b);
        double Subtract(double a, double b);
        double Multiply(double a, double b);
        double Divide(double a, double b);   // DivideByZeroException при b == 0
        double Power(double a, double b);
        double SquareRoot(double a);          // ArgumentException при a < 0
        double Square(double a);
        double Sin(double degrees);
        double Cos(double degrees);
        double Tan(double degrees);
        double Log10(double a);               // ArgumentException при a <= 0
        double Ln(double a);                  // ArgumentException при a <= 0
    }
}