using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using WpfApp2.Models;
using WpfApp2.Services.Interfaces;

namespace WpfApp2.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly string _filePath;

        public HistoryService()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CalculatorApp");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _filePath = Path.Combine(dir, "calculator_history.json");
        }

        public ObservableCollection<HistoryItem> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new ObservableCollection<HistoryItem>();

                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<ObservableCollection<HistoryItem>>(json)
                       ?? new ObservableCollection<HistoryItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка загрузки истории: " + ex.Message);
                return new ObservableCollection<HistoryItem>();
            }
        }

        public void Save(ObservableCollection<HistoryItem> items)
        {
            try
            {
                var json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка сохранения истории: " + ex.Message);
            }
        }
    }
}