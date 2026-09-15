using System;
using System.Collections.ObjectModel;
using WpfApp2.Models;
using WpfApp2.Services.Interfaces;

namespace WpfApp2.ViewModels
{
    public class MainViewModel
    {
        private readonly ICalculatorService _calc;
        private readonly IHistoryService _history;

        // --- Состояние ---
        private string _currentInput = "0";
        private string _currentExpression = "";
        private bool _isNewInput = true;
        private string _lastOperator = "";
        private double _lastValue = 0;
        private bool _isError = false;

        // --- Публичные свойства (только чтение снаружи) ---
        public string CurrentInput => _currentInput;
        public string Expression => string.IsNullOrEmpty(_currentExpression) ? "0" : _currentExpression;
        public bool IsEngineerMode { get; set; } = false;
        public ObservableCollection<HistoryItem> History { get; }

        public MainViewModel(ICalculatorService calc, IHistoryService history)
        {
            _calc = calc;
            _history = history;
            History = _history.Load();
        }

        // --- Главный метод: вызывается из code-behind по нажатию кнопки ---
        public void OnButton(string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            if (_isError)
            {
                ClearAll();
                _isError = false;
            }

            switch (content)
            {
                case "C": ClearAll(); break;
                case "±": ToggleSign(); break;
                case "%": ApplyPercent(); break;
                case "÷": SetOperator("/"); break;
                case "×": SetOperator("*"); break;
                case "-": SetOperator("-"); break;
                case "+": SetOperator("+"); break;
                case "=": CalculateResult(); break;
                case "√": SquareRoot(); break;
                case "x²": Square(); break;
                case "xʸ": SetOperator("^"); break;
                case "sin": TrigFunc("sin", _calc.Sin); break;
                case "cos": TrigFunc("cos", _calc.Cos); break;
                case "tan": TrigFunc("tan", _calc.Tan); break;
                case "log": LogFunc("log", _calc.Log10); break;
                case "ln": LogFunc("ln", _calc.Ln); break;
                case "π": AddConstant(Math.PI); break;
                case "e": AddConstant(Math.E); break;
                case "(": AddToExpression("("); break;
                case ")": AddToExpression(")"); break;
                default:
                    if (IsDigit(content)) AddDigit(content);
                    break;
            }
        }

        public void ClearHistory()
        {
            History.Clear();
            _history.Save(History);
        }

        public void ApplyHistoryItem(HistoryItem item)
        {
            if (item == null) return;
            if (double.TryParse(item.Result, out double value))
            {
                _currentInput = value.ToString();
                _currentExpression = _currentInput;
                _isNewInput = true;
                _lastOperator = "";
            }
        }

        // --- Внутренняя логика (та же, что была в MainWindow) ---

        private bool IsDigit(string c) =>
            c == "0" || c == "1" || c == "2" || c == "3" ||
            c == "4" || c == "5" || c == "6" || c == "7" ||
            c == "8" || c == "9" || c == "00" || c == ",";

        private void AddDigit(string digit)
        {
            if (_isNewInput)
            {
                _currentInput = (digit == ",") ? "0," : digit;
                _isNewInput = false;
            }
            else
            {
                if (digit == "," && _currentInput.Contains(",")) return;

                if (_currentInput == "0" && digit != ",")
                    _currentInput = digit;
                else
                    _currentInput += digit;
            }
            _currentExpression += digit;
        }

        private void SetOperator(string op)
        {
            if (!string.IsNullOrEmpty(_currentInput) && !_isNewInput)
            {
                if (!string.IsNullOrEmpty(_lastOperator))
                    CalculateResult();

                _lastValue = double.Parse(_currentInput);
                _lastOperator = op;

                string symbol = op == "/" ? "÷" : (op == "*" ? "×" : op);
                _currentExpression = _currentExpression + " " + symbol + " ";
                _isNewInput = true;
            }
        }

        private void CalculateResult()
        {
            if (string.IsNullOrEmpty(_lastOperator) || _isNewInput) return;

            double currentValue = double.Parse(_currentInput);
            double result;

            try
            {
                switch (_lastOperator)
                {
                    case "+": result = _calc.Add(_lastValue, currentValue); break;
                    case "-": result = _calc.Subtract(_lastValue, currentValue); break;
                    case "*": result = _calc.Multiply(_lastValue, currentValue); break;
                    case "/": result = _calc.Divide(_lastValue, currentValue); break;
                    case "^": result = _calc.Power(_lastValue, currentValue); break;
                    default: return;
                }

                AddToHistory(_currentExpression.Trim(), result.ToString());

                _currentInput = result.ToString();
                _currentExpression = result.ToString();
                _lastOperator = "";
                _isNewInput = true;
            }
            catch (DivideByZeroException)
            {
                _currentInput = "Ошибка: деление на 0";
                _isError = true;
            }
            catch (Exception)
            {
                _currentInput = "Ошибка";
                _isError = true;
            }
        }

        private void ClearAll()
        {
            _currentInput = "0";
            _currentExpression = "";
            _lastOperator = "";
            _lastValue = 0;
            _isNewInput = true;
            _isError = false;
        }

        private void ToggleSign()
        {
            if (double.TryParse(_currentInput, out double value))
            {
                _currentInput = (-value).ToString();
                _currentExpression = _currentInput;
            }
        }

        private void ApplyPercent()
        {
            if (double.TryParse(_currentInput, out double value))
            {
                _currentInput = (value / 100).ToString();
                _currentExpression = _currentInput;
            }
        }

        private void SquareRoot()
        {
            if (double.TryParse(_currentInput, out double value) && value >= 0)
            {
                try
                {
                    double result = _calc.SquareRoot(value);
                    string expr = "√(" + _currentExpression + ")";
                    _currentInput = result.ToString();
                    _currentExpression = _currentInput;
                    AddToHistory(expr, _currentInput);
                    _isNewInput = true;
                }
                catch (ArgumentException)
                {
                    _currentInput = "Ошибка";
                    _isError = true;
                }
            }
        }

        private void Square()
        {
            if (double.TryParse(_currentInput, out double value))
            {
                double result = _calc.Square(value);
                string expr = "(" + _currentExpression + ")²";
                _currentInput = result.ToString();
                _currentExpression = _currentInput;
                AddToHistory(expr, _currentInput);
                _isNewInput = true;
            }
        }

        private void TrigFunc(string name, Func<double, double> func)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                double result = func(value);
                string expr = name + "(" + _currentExpression + ")";
                _currentInput = result.ToString();
                _currentExpression = _currentInput;
                AddToHistory(expr, _currentInput);
                _isNewInput = true;
            }
        }

        private void LogFunc(string name, Func<double, double> func)
        {
            if (double.TryParse(_currentInput, out double value) && value > 0)
            {
                try
                {
                    double result = func(value);
                    string expr = name + "(" + _currentExpression + ")";
                    _currentInput = result.ToString();
                    _currentExpression = _currentInput;
                    AddToHistory(expr, _currentInput);
                    _isNewInput = true;
                }
                catch (ArgumentException)
                {
                    _currentInput = "Ошибка";
                    _isError = true;
                }
            }
        }

        private void AddConstant(double constant)
        {
            _currentInput = constant.ToString();
            _currentExpression = _currentInput;
            _isNewInput = true;
        }

        private void AddToExpression(string value)
        {
            _currentExpression += value;
            _currentInput = _isNewInput ? value : _currentInput + value;
            _isNewInput = false;
        }

        private void AddToHistory(string expression, string result)
        {
            History.Insert(0, new HistoryItem
            {
                Expression = expression,
                Result = result,
                Timestamp = DateTime.Now
            });

            while (History.Count > 100)
                History.RemoveAt(History.Count - 1);

            _history.Save(History);
        }
    }
}