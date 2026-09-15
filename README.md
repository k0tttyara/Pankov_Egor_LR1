# Инженерный калькулятор (WPF)

## Назначение
Desktop-приложение на WPF: обычный и инженерный режимы, история вычислений с сохранением в JSON.

## Технологии
C#, .NET Framework, WPF, Newtonsoft.Json. Архитектура — упрощённый MVVM.

## Структура проекта
- **Views/** — окна (MainWindow.xaml + .cs)
- **ViewModels/** — логика калькулятора (MainViewModel)
- **Models/** — модель данных (HistoryItem)
- **Services/** — CalculatorService, HistoryService
- **Services/Interfaces/** — интерфейсы сервисов

| Каталог | Назначение |
|---------|-----------|
| Views/ | XAML-разметка, привязка событий к VM |
| ViewModels/ | Состояние и логика калькулятора |
| Models/ | Сущности (HistoryItem) |
| Services/ | Математика и работа с JSON-файлом |

## Архитектура
Логика вынесена из окна в ViewModel, математика — в CalculatorService, работа с файлом — в HistoryService. Code-behind отвечает только за привязку событий UI к методам VM.

## Запуск
1. Открыть WpfApp2.sln в Visual Studio.
2. Ctrl + Shift + B — сборка.
3. F5 — запуск.

История хранится в %LocalAppData%\CalculatorApp\calculator_history.json.

## Автор
_Панков Егор Владимирович, 2026_
