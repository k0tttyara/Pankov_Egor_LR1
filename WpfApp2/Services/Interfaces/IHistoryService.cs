using System.Collections.ObjectModel;
using WpfApp2.Models;

namespace WpfApp2.Services.Interfaces
{
    public interface IHistoryService
    {
        ObservableCollection<HistoryItem> Load();
        void Save(ObservableCollection<HistoryItem> items);
    }
}