using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2.Models;
using WpfApp2.Services;
using WpfApp2.ViewModels;

namespace WpfApp2.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel(new CalculatorService(), new HistoryService());

            // Подписываем все кнопки калькулятора на один обработчик
            SubscribeButtons();

            EngineerToggle.Checked += (s, e) =>
            {
                _vm.IsEngineerMode = true;
                EngineerPanel.Visibility = Visibility.Visible;
            };
            EngineerToggle.Unchecked += (s, e) =>
            {
                _vm.IsEngineerMode = false;
                EngineerPanel.Visibility = Visibility.Collapsed;
            };

            ClearHistoryBtn.Click += ClearHistory_Click;
            HistoryList.MouseDoubleClick += HistoryList_MouseDoubleClick;

            UpdateDisplay();
        }

        private void SubscribeButtons()
        {
            Btn0.Click += Button_Click; Btn1.Click += Button_Click;
            Btn2.Click += Button_Click; Btn3.Click += Button_Click;
            Btn4.Click += Button_Click; Btn5.Click += Button_Click;
            Btn6.Click += Button_Click; Btn7.Click += Button_Click;
            Btn8.Click += Button_Click; Btn9.Click += Button_Click;
            Btn00.Click += Button_Click; BtnComma.Click += Button_Click;

            BtnClear.Click += Button_Click; BtnSign.Click += Button_Click;
            BtnPercent.Click += Button_Click; BtnDivide.Click += Button_Click;
            BtnMultiply.Click += Button_Click; BtnMinus.Click += Button_Click;
            BtnPlus.Click += Button_Click; BtnEquals.Click += Button_Click;

            BtnSin.Click += Button_Click; BtnCos.Click += Button_Click;
            BtnTan.Click += Button_Click; BtnSqrt.Click += Button_Click;
            BtnSquare.Click += Button_Click; BtnPow.Click += Button_Click;
            BtnLog.Click += Button_Click; BtnLn.Click += Button_Click;
            BtnPi.Click += Button_Click; BtnE.Click += Button_Click;
            BtnOpenParen.Click += Button_Click; BtnCloseParen.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            _vm.OnButton(button.Content?.ToString());
            UpdateDisplay();
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Очистить всю историю вычислений?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _vm.ClearHistory();
                UpdateDisplay();
            }
        }

        private void HistoryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = HistoryList.SelectedItem as HistoryItem;
            if (item == null) return;

            _vm.ApplyHistoryItem(item);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            ExpressionPreview.Text = _vm.Expression;
            ResultDisplay.Text = _vm.CurrentInput;
            HistoryList.ItemsSource = _vm.History;
        }
    }
}