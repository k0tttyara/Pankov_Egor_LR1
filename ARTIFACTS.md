# Артефакты и протоколы проекта

## 1. Введение

Документ описывает состав артефактов и протоколы взаимодействия между
модулями проекта «Инженерный калькулятор» (WPF). Область применения -
командная разработка, интеграция модулей, сопровождение проекта.

## 2. Перечень артефактов

| Имя | Тип | Категория | Назначение | В Git | Расположение |
|-----|-----|-----------|-----------|-------|--------------|
| WpfApp2.sln | вспомогательный | проект | файл решения | да | корень |
| WpfApp2.csproj | исходный | проект | описание сборки | да | корень |
| App.xaml | исходный | код | разметка точки входа | да | корень |
| App.xaml.cs | исходный | код | code-behind точки входа | да | корень |
| App.config | конфигурация | настройки | конфигурация .NET | да | корень |
| packages.config | конфигурация | настройки | список NuGet-пакетов | да | корень |
| Views/MainWindow.xaml | исходный | код | разметка главного окна | да | Views/ |
| Views/MainWindow.xaml.cs | исходный | код | привязка событий UI к VM | да | Views/ |
| ViewModels/MainViewModel.cs | исходный | код | логика калькулятора | да | ViewModels/ |
| Models/HistoryItem.cs | исходный | код | модель записи истории | да | Models/ |
| Services/CalculatorService.cs | исходный | код | математические операции | да | Services/ |
| Services/HistoryService.cs | исходный | код | загрузка/сохранение истории | да | Services/ |
| Services/Interfaces/ICalculatorService.cs | исходный | код | интерфейс сервиса вычислений | да | Services/Interfaces/ |
| Services/Interfaces/IHistoryService.cs | исходный | код | интерфейс сервиса истории | да | Services/Interfaces/ |
| README.md | документация | проект | описание проекта | да | корень |
| ARTIFACTS.md | документация | проект | артефакты и протоколы | да | корень |
| .gitignore | конфигурация | Git | исключения Git | да | корень |
| bin/ | производный | сборка | результаты сборки | нет | корень |
| obj/ | производный | сборка | промежуточные файлы | нет | корень |
| .vs/ | производный | IDE | кэш Visual Studio | нет | корень |

## 3. Протоколы взаимодействия

### Протокол: ICalculatorService
Участники: ViewModels - Services
Назначение: предоставление математических операций.
Интерфейс:
- double Add(double a, double b);
- double Subtract(double a, double b);
- double Multiply(double a, double b);
- double Divide(double a, double b);
- double Power(double a, double b);
- double SquareRoot(double a);
- double Square(double a);
- double Sin(double degrees);
- double Cos(double degrees);
- double Tan(double degrees);
- double Log10(double a);
- double Ln(double a);
Формат данных: примитивы double.
Ограничения: Divide бросает DivideByZeroException; SquareRoot, Log10, Ln -
ArgumentException.
Соглашения: синхронные методы, PascalCase, глагол + существительное.

### Протокол: IHistoryService
Участники: ViewModels - Services
Назначение: загрузка и сохранение истории вычислений.
Интерфейс:
- ObservableCollection<HistoryItem> Load();
- void Save(ObservableCollection<HistoryItem> items);
Формат данных: коллекция объектов HistoryItem, сериализация в JSON.
Файл: %LocalAppData%\CalculatorApp\calculator_history.json.
Соглашения: синхронные методы; ошибки ввода-вывода логируются и не пробрасываются.

### Протокол: View - ViewModel
Участники: Views - ViewModels
Назначение: передача нажатий пользователя и отображение состояния.
Интерфейс:
- void OnButton(string content);
- void ClearHistory();
- void ApplyHistoryItem(HistoryItem item);
- string CurrentInput { get; }
- string Expression { get; }
- bool IsEngineerMode { get; set; }
- ObservableCollection<HistoryItem> History { get; }
Формат данных: строки, объект HistoryItem.
Соглашения: code-behind без бизнес-логики.

### Протокол: события UI
Участники: Views - ViewModels
Назначение: маршрутизация действий пользователя.
События: Click кнопок, MouseDoubleClick по HistoryList,
Checked/Unchecked у EngineerToggle.
Формат: стандартные RoutedEventArgs, MouseButtonEventArgs.
Соглашения: подписка в MainWindow.xaml.cs, вызов методов VM.

## 4. Соглашения об именовании и версионировании

| Элемент | Правило | Пример |
|---------|---------|--------|
| Классы | PascalCase | MainViewModel |
| Интерфейсы | I + PascalCase | ICalculatorService |
| Методы | PascalCase | CalculateResult |
| Приватные поля | _camelCase | _currentInput |
| Свойства | PascalCase | CurrentInput |
| Файлы | = имени класса | MainViewModel.cs |
| Namespace | = путь каталога | WpfApp2.ViewModels |
| Ветки Git | feature/, fix/ | feature/themes |
| Коммиты | тип: описание | feat: add ARTIFACTS.md |

Версионирование - SemVer (MAJOR.MINOR.PATCH).
Текущая версия проекта - 0.1.0.

## 5. Заключение

Документ фиксирует состав артефактов и протоколы взаимодействия модулей
проекта «Инженерный калькулятор». Единые правила именования и
версионирования упрощают командную работу и интеграцию модулей.
